using Microsoft.Extensions.Logging;
using ZM.Generator;
using ZM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


using NodeDictionary = System.Collections.Generic.Dictionary<System.Type, ZM.Models.Node>;
using System.Diagnostics;

namespace ZM
{
    public class Codegen
    {
        ILogger<Codegen> _logger;

        public Codegen(ILogger<Codegen> logger)
        {
            _logger = logger;
        }

        public void Generate(Assembly assembly)
        {
            _logger.LogInformation($"Generating code for: {assembly.FullName} [{assembly.GetExportedTypes().Length} types discovered]");

            NodeRegistry registry = new NodeRegistry();

            BuiltInNodes.ForEach(node => registry.Nodes.Add(node.SourceType, node));
            Array.ForEach(assembly.GetExportedTypes(), t => registry.Register(t));

            String projectName = $"{assembly.GetName().Name}.Generated";
            String outputFolder = $@"..\..\..\..\{projectName}";

            // registry is sealed at this point

            Array.ForEach(assembly.GetExportedTypes(), t =>
            {
                Generate(new TemplateContext { CurrentNode = registry.Nodes[t], Registry = registry }, outputFolder);
            });

            RegenerateProject(projectName, outputFolder);
        }

        void RegenerateProject(String projectName, String outputFolder)
        {
            _logger.LogInformation($"Generating [{projectName}] => {Path.GetFullPath(outputFolder)}\\CMakeLists.txt");
            String cmakeContent = new CMakeListsTemplate() { ProjectName = projectName }.TransformText();
            File.WriteAllText(Path.Combine(outputFolder, $"CMakeLists.txt"), cmakeContent);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\CommonExtensions\Microsoft\CMake\CMake\bin\cmake.exe", // VS TODO: Fix absolute path
                    Arguments = ". -G \"Visual Studio 16 2019\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.GetFullPath(outputFolder),
                }
            };

            _logger.LogInformation($"{process.StartInfo.FileName} {process.StartInfo.Arguments}");
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            if (!String.IsNullOrEmpty(output))
                _logger.LogInformation($"cmake output:\n{output}");

            string err = process.StandardError.ReadToEnd();
            if (!String.IsNullOrWhiteSpace(err))
                _logger.LogError($"cmake error:\n{err}");

            process.WaitForExit();
        }

        static List<Node> BuiltInNodes = new List<Node>()
        {
            new Node(typeof(byte), "uint8_t", "<cstdint>"),
            new Node(typeof(UInt16), "uint16_t", "<cstdint>"),
            new Node(typeof(UInt32), "uint32_t", "<cstdint>"),
            new Node(typeof(UInt64), "uint64_t", "<cstdint>"),
            new Node(typeof(Int16), "int16_t", "<cstdint>"),
            new Node(typeof(Int32), "int32_t", "<cstdint>"),
            new Node(typeof(Int64), "int64_t", "<cstdint>"),
            new Node(typeof(float), "float", "<cstdint>"),
            new Node(typeof(double), "double", "<cstdint>"),

            new Node(typeof(String), "zm::String", "Zmeya.h"),
        };

        

        void Generate(TemplateContext context, String outputFolder)
        {
            _logger.LogInformation($"Generating [{context.CurrentNode.SourceType.FullName}] => {Path.GetFullPath(outputFolder)}\\{context.CurrentNode.TargetType}.h(.cpp)");
            
            if (!String.IsNullOrWhiteSpace(outputFolder))
                Directory.CreateDirectory(outputFolder);

            String headerContent = new HeaderTemplate() { Context = context }.TransformText();
            File.WriteAllText(Path.Combine(outputFolder, $"{context.CurrentNode.TargetType}.h"), headerContent);

            String cppContent = new CPPTemplate() { Context = context }.TransformText();
            File.WriteAllText(Path.Combine(outputFolder, $"{context.CurrentNode.TargetType}.cpp"), cppContent);
        }
    }
}
