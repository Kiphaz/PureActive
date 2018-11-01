﻿using System.Text.RegularExpressions;
using PureActive.Core.Abstractions.Extensions;

namespace PureActive.Core.Utilities
{
    public class FullNameParser
    {
        public FullNameParser(string fullName)
        {
            FullName = fullName;
            _ParseFullName(fullName);
        }

        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PreferredName { get; set; }

        private void _ParseParensPreferredName(string fullNamePreferred)
        {
            var mc = Regex.Match(fullNamePreferred, @"\A^(.*)[\s]?\((.*)\)[\s](.*)$\z").Groups;

            if (mc.Count == 4)
            {
                FirstName = mc[1].Value.Trim();
                PreferredName = mc[2].Value.Replace("\"", "").Trim();
                LastName = mc[3].Value.Trim();
            }
            else if (mc.Count == 3)
            {
                FirstName = mc[1].Value.Trim();
                LastName = mc[2].Value.Trim();
            }
            else if (mc.Count == 2)
            {
                FirstName = mc[1].Value.Trim();
            }
        }


        private void _ParseFullName(string fullName)
        {
            if (!string.IsNullOrEmpty(fullName))
            {
                if (fullName.IndexOf(',') != -1)
                {
                    // Last, First
                    var strings = fullName.SplitOnLastDelim(',');

                    if (strings != null && strings.Length == 2)
                    {
                        LastName = strings[0].Trim();
                        FirstName = strings[1].Trim();

                        FullName = $"{FirstName} {LastName}";

                        // Parse Preferred Name from Normalized FullName
                        if (fullName.IndexOf('(') != -1)
                            _ParseParensPreferredName(FullName);
                    }
                    else
                    {
                        FirstName = fullName;
                    }
                }
                // Preferred Name in ()
                else if (fullName.IndexOf('(') == -1)
                {
                    var strings = fullName.SplitOnLastDelim(' ');

                    if (strings != null && strings.Length == 2)
                    {
                        FirstName = strings[0];
                        LastName = strings[1];
                    }
                    else
                    {
                        FirstName = fullName;
                    }
                }
                else
                {
                    _ParseParensPreferredName(fullName);
                }
            }
        }
    }
}