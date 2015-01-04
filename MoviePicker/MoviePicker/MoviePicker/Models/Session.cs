using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MoviePicker.Data;
using Shared.Models;

namespace MoviePicker.Models
{
    public class Session
    { 
        public User User { get; set; }

        private readonly DataSource _dataSource = new DataSource();
        public DataSource DataSource
        {
            get { return _dataSource; }
        }

        public string DatabasePath
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "database.db3"); }
        }

        public bool IsLoggedIn
        {
            get { return User != null; }
        }
    }
}
