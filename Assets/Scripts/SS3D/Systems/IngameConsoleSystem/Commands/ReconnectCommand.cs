﻿using System.Diagnostics;
using FishNet.Connection;
using SS3D.Permissions;
using UnityEngine.Device;

namespace SS3D.Systems.IngameConsoleSystem.Commands
{
    public class ReconnectCommand: Command
    {
        public override string LongDescription => "Restart app";
        public override string ShortDescription => "Restart app";
        public override ServerRoleTypes AccessLevel => ServerRoleTypes.User;
        public override CommandType Type => CommandType.Offline;

        public override string Perform(string[] args, NetworkConnection conn = null)
        {
            CheckArgsResponse checkArgsResponse = CheckArgs(args);
            if (checkArgsResponse.IsValid == false)
                return checkArgsResponse.InvalidArgs;
            
            Process.Start(Application.dataPath.Replace("_Data", ".exe"));
            Application.Quit();
            return "Done";
        }
        protected override CheckArgsResponse CheckArgs(string[] args)
        {
            CheckArgsResponse response = new CheckArgsResponse();
            if (args.Length != 0)
            {
                response.IsValid = false;
                response.InvalidArgs = "Invalid number of arguments";
                return response;
            }
            
            response.IsValid = true;
            return response;
        }
    }
}