namespace Boxed.DotnetNewTest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// <see cref="TempDirectory"/> extension methods.
    /// </summary>
    public static class TempDirectoryExtensions
    {
        /// <summary>
        /// Runs 'dotnet new' with the specified arguments.
        /// </summary>
        /// <param name="tempDirectory">The temporary directory.</param>
        /// <param name="templateName">Name of the 'dotnet new' template to create.</param>
        /// <param name="name">The name of the project to create from the template.</param>
        /// <param name="arguments">The custom arguments to pass to the template.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A project created from a project template.</returns>
        public static async Task<Project> DotnetNew(
            this TempDirectory tempDirectory,
            string templateName,
            string name,
            IDictionary<string, string> arguments = null,
            TimeSpan? timeout = null)
        {
            var stringBuilder = new StringBuilder($"new {templateName} --name \"{name}\"");
            if (arguments != null)
            {
                foreach (var argument in arguments)
                {
                    stringBuilder.Append($" --{argument.Key} \"{argument.Value}\"");
                }
            }

            await ProcessExtensions.StartAsync(
                tempDirectory.DirectoryPath,
                "dotnet",
                stringBuilder.ToString(),
                CancellationTokenFactory.GetCancellationToken(timeout));

            var projectDirectoryPath = Path.Combine(tempDirectory.DirectoryPath, name);
            var publishDirectoryPath = Path.Combine(projectDirectoryPath, "Publish");
            return new Project(name, projectDirectoryPath, publishDirectoryPath);
        }
    }
}