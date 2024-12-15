using System.Reactive;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia;
using AvaloniaEdit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI;
using voicio.Models;
using System.IO;

namespace voicio.ViewModels
{
    public class CodeWindowViewModel : ViewModelBase
    {
        public VoiceOperation bindedObject;
        private string _TitleText = "";
        public string TitleText
        {
            get => _TitleText;
            set => this.RaiseAndSetIfChanged(ref _TitleText, value);
        }
        private AvaloniaEdit.Document.TextDocument _SourceCode;
        public AvaloniaEdit.Document.TextDocument SourceCode
        {
            get => _SourceCode;
            set => this.RaiseAndSetIfChanged(ref _SourceCode, value);
        }
        public void Compile()
        {

            var options = new CSharpCompilationOptions((OutputKind)LanguageVersion.Latest);
            var syntaxTree = CSharpSyntaxTree.ParseText(bindedObject.SourceCode);

            var compilation = CSharpCompilation.Create("DynamicAssembly")
            .AddSyntaxTrees(syntaxTree)
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);
                bindedObject.ActionTreeExpression = ms.ToArray();
                if (bindedObject.IsSaved)
                {
                    using (var DataSource = new HelpContext())
                    {
                        DataSource.VoiceOperationTable.Attach(bindedObject);
                        DataSource.VoiceOperationTable.Update(bindedObject);
                        DataSource.SaveChanges();
                    }
                }
                else
                {
                    using (var DataSource = new HelpContext())
                    {
                        DataSource.VoiceOperationTable.Attach(bindedObject);
                        DataSource.VoiceOperationTable.Add(bindedObject);
                        DataSource.SaveChanges();
                        bindedObject.IsSaved = true;
                    }
                }
            }
        }
        public ReactiveCommand<Unit, Unit> CompileCommand { get; }
        public CodeWindowViewModel(VoiceOperation obj) {
            CompileCommand = ReactiveCommand.Create(Compile);
            bindedObject = obj;
            if (obj.SourceCode is null) obj.SourceCode = "";
            SourceCode = new AvaloniaEdit.Document.TextDocument(obj.SourceCode);
            TitleText = "Code for \"" + obj.Command + "\" command...";
        }
    }
}
