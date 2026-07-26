using System.Diagnostics;
using System.IO;
using System.Text;

namespace PrologCoder.Services
{
    public class CompilerService
    {
        private readonly string _compilerPath;
        private Process? _process;

        public CompilerService()
        {
            _compilerPath = Path.Combine(
                AppContext.BaseDirectory,
                "Compiler",
                "SWI Prolog",
                "swipl.exe");
        }

        public event Action? ProcessStarted;
        public event Action? ProcessExited;
        public event Action<string>? OutputReceived;
        public event Action<string>? ErrorReceived;

        public void Run(string filePath, string goal = "main")
        {
            if (_process != null)
                return;

            if (!File.Exists(_compilerPath))
            {
                ErrorReceived?.Invoke($"Компилятор не найден:\n{_compilerPath}");
                return;
            }

            if (!File.Exists(filePath))
            {
                ErrorReceived?.Invoke($"Файл не найден:\n{filePath}");
                return;
            }

            _process = new Process();

            _process.StartInfo.FileName = _compilerPath;
            _process.StartInfo.Arguments = $"-q -s \"{filePath}\" -g {goal} -t halt";

            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.CreateNoWindow = true;

            _process.StartInfo.RedirectStandardInput = true;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardError = true;

            _process.StartInfo.StandardInputEncoding = new UTF8Encoding(false);
            _process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            _process.StartInfo.StandardErrorEncoding = Encoding.UTF8;

            _process.EnableRaisingEvents = true;

            _process.Exited += (_, _) =>
            {
                ProcessExited?.Invoke();

                _process?.Dispose();
                _process = null;
            };

            _process.Start();

            ProcessStarted?.Invoke();

            _ = ReadOutputAsync(_process);
            _ = ReadErrorAsync(_process);
        }

        private async Task ReadOutputAsync(Process process)
        {
            char[] buffer = new char[512];

            try
            {
                while (true)
                {
                    int count = await process.StandardOutput.ReadAsync(buffer, 0, buffer.Length);

                    if (count == 0)
                        break;

                    string text = new string(buffer, 0, count);

                    OutputReceived?.Invoke(text);
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }

        private async Task ReadErrorAsync(Process process)
        {
            char[] buffer = new char[512];

            try
            {
                while (true)
                {
                    int count = await process.StandardError.ReadAsync(buffer, 0, buffer.Length);

                    if (count == 0)
                        break;

                    string text = new string(buffer, 0, count);

                    ErrorReceived?.Invoke(text);
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }

        public void SendInput(string text)
        {
            if (_process == null)
                return;

            _process.StandardInput.WriteLine(text);
            _process.StandardInput.Flush();
        }

        public void Stop()
        {
            if (_process == null)
                return;

            try
            {
                if (!_process.HasExited)
                    _process.Kill(true);
                _process.StandardInput.Close();
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}
