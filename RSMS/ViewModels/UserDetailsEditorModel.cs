﻿using Domain.Models;

namespace RSMS.ViewModels
{
    public class UserDetailsEditorModel
    {
        public UserInfo UserInfo { get; set; }
        public List<string> AllRoles { get; set; }
    }
}