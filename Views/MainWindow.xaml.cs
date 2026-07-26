using Microsoft.Win32;
using PrologCoder.Models;
using PrologCoder.Services;
using System.ComponentModel;
using System.Windows;

namespace PrologCoder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _ignoreTextChanged = false;

        private readonly FileService _fileService = new();
        private readonly CompilerService _compilerService = new();

        private Document _document = new(); 

        public MainWindow()
        {
            InitializeComponent();
            InitializeTextEditor();

            _compilerService.ProcessStarted += () =>
            {
                Dispatcher.Invoke(() =>
                {
                    Run.IsEnabled = false;
                    Stop.IsEnabled = true;
                    editor.IsReadOnly = true;
                    output.StartSession();
                });
            };
            _compilerService.OutputReceived += text =>
            {
                Dispatcher.Invoke(() =>
                {
                    output.AppendOutput(text);
                });
            };
            _compilerService.ErrorReceived += text =>
            {
                Dispatcher.Invoke(() =>
                {
                    output.AppendOutput(text);
                });
            };
            _compilerService.ProcessExited += () =>
            {
                Dispatcher.Invoke(() =>
                {
                    Run.IsEnabled = true;
                    Stop.IsEnabled = false;
                    editor.IsReadOnly = false;

                    output.AppendOutput($"\n\nProgram {_document.FileName} has ended");
                    output.StopSession();
                });
            };

            output.InputSended += _compilerService.SendInput;
        }

        private void InitializeTextEditor()
        {
            editor.Options.ConvertTabsToSpaces = true;
            editor.Options.HighlightCurrentLine = true;
            editor.Options.IndentationSize = 4;
            editor.Options.EnableHyperlinks = false;

            editor.TextArea.Caret.PositionChanged += (s, e) =>
            {
                sbLine.Content = $"Line: {editor.TextArea.Caret.Line}";
                sbColumn.Content = $"Column: {editor.TextArea.Caret.Column}";
            };
        }

        private bool SaveDocument()
        {
            if (_document.FilePath == null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Prolog files (*.pl)|*.pl|All files (*.*)|*.*";

                if (saveFileDialog.ShowDialog() == true)
                {
                    _document.FilePath = saveFileDialog.FileName;
                }
                else return false;
            }

            _fileService.SaveFile(_document.FilePath, editor.Text);
            sbCurrentFile.Content = _document.FileName;
            _document.IsModified = false;

            return true;
        }

        private bool SaveIfModified()
        {
            if (_document.IsModified)
            {
                var messageBoxResult = MessageBox.Show("У вас есть несохраненные изменения. Сохранить?",
                                "Несохраненные изменения",
                                MessageBoxButton.YesNoCancel,
                                MessageBoxImage.Warning);

                if (messageBoxResult == MessageBoxResult.Yes) return SaveDocument();
                if (messageBoxResult == MessageBoxResult.Cancel) return false;
            }

            return true;
        }

        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            if (!SaveIfModified()) return;

            _document = new Document();
            editor.Text = "";
            sbCurrentFile.Content = _document.FileName;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (!SaveIfModified()) return;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Prolog files (*.pl)|*.pl|All files (*.*)|*.*";
            
            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;

                _document = new Document
                {
                    FilePath = path,
                    Text = _fileService.OpenFile(path),
                    IsModified = false
                };

                _ignoreTextChanged = true;
                editor.Text = _document.Text;
                _ignoreTextChanged = false;
                sbCurrentFile.Content = _document.FileName;
                _document.IsModified = false;
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveDocument();
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            if (!SaveDocument()) return;

            output.ClearTerminal();
            output.AppendOutput($"Running {_document.FileName}\n\n");
            _compilerService.Run(_document.FilePath!);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _compilerService.Stop();
        }

        private void editor_TextChanged(object sender, EventArgs e)
        {
            if (_ignoreTextChanged) return;
            _document.IsModified = true;
            sbCurrentFile.Content = $"{_document.FileName}*";
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _compilerService.Stop();

            base.OnClosing(e);
        }
    }
}