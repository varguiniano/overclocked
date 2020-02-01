using System.Collections.Generic;
using UnityEngine;
using Varguiniano.Core.Runtime.Common;

namespace Varguiniano.Core.Runtime.Debug.CommandProcessors
{
    /// <summary>
    /// Base class for any class that listens to commands from the in game console and process them.
    /// </summary>
    public abstract class InGameCommandProcessor : MonoBehaviour
    {
        /// <summary>
        /// Name of the module being represented by this command processor.
        /// This name is ignored if this processor is an extension.
        /// </summary>
        public abstract string ModuleName { get; }

        /// <summary>
        /// Extensions of this command processor.
        /// This extensions will act under the same module name and will be called to process the command if this processor can't do it.
        /// </summary>
        public InGameCommandProcessor[] Extensions;

        /// <summary>
        /// Process the command entered on the console.
        /// </summary>
        /// <param name="message">The command to process.</param>
        public void EnterCommand(string message)
        {
            string[] moduleAndCommand = message.Split('.');

            if (moduleAndCommand.Length == 1)
            {
                NoCommand();
                return;
            }

            string command = moduleAndCommand[1];

            if (string.IsNullOrEmpty(command) || string.IsNullOrWhiteSpace(command))
            {
                NoCommand();
                return;
            }

            string[] commandAndParams = command.Split('(');

            if (commandAndParams.Length == 1)
            {
                NoCommand();
                return;
            }

            string method = commandAndParams[0];

            if (string.IsNullOrEmpty(method) || string.IsNullOrWhiteSpace(method))
            {
                NoCommand();
                return;
            }

            string parametersAndParenthesis = commandAndParams[1];

            if (!parametersAndParenthesis.EndsWith(")"))
            {
                NoCommand();
                return;
            }

            parametersAndParenthesis = parametersAndParenthesis.RemoveLastChar();

            string[] rawParameters = parametersAndParenthesis.Split(',');
            List<string> parameters = new List<string>();

            for (int i = 0; i < rawParameters.Length; ++i)
            {
                rawParameters[i] = rawParameters[i].Replace(" ", "");

                if (!string.IsNullOrEmpty(rawParameters[i])) parameters.Add(rawParameters[i]);
            }

            bool commandProcessed = ProcessCommand(method, parameters.ToArray());

            if (!commandProcessed)
                for (int i = 0; i < Extensions.Length; ++i)
                    if (Extensions[i].ProcessCommand(method, parameters.ToArray()))
                    {
                        commandProcessed = true;
                        break;
                    }

            if (!commandProcessed) NotRecognized(command);
        }

        /// <summary>
        /// Process the command entered on the console.
        /// </summary>
        /// <param name="command">The command to process.</param>
        /// <param name="parameters">Parameters.</param>
        protected abstract bool ProcessCommand(string command, params string[] parameters);

        /// <summary>
        /// Send a not command message.
        /// </summary>
        private void NoCommand() =>
            Logger.LogError("You have to specify a command such as " + ModuleName + ".Method(param,param).",
                            ModuleName);

        /// <summary>
        /// Send a not recognized message.
        /// </summary>
        /// <param name="command">The command not recognized.</param>
        private void NotRecognized(string command) =>
            Logger.LogError("Command " + command + " not found on this module.", ModuleName);

        /// <summary>
        /// Send a wrong params message.
        /// </summary>
        /// <param name="command">The command called.</param>
        protected void WrongParams(string command) =>
            Logger.LogError("Command " + command + " has been called with wrong parameters.", ModuleName);

        /// <summary>
        /// Send a wrong param number.
        /// </summary>
        /// <param name="command">Command called.</param>
        /// <param name="number">Number of params called.</param>
        /// <param name="target">Target number of params.</param>
        protected void WrongParamNumber(string command, int number, int target) =>
            Logger.LogError("Command "
                          + command
                          + " called with "
                          + number
                          + " params. It must be called with "
                          + target
                          + ".",
                            ModuleName);
    }
}