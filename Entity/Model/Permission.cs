﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<FormRolPermission> FormRolPermissions { get; set; } = new List<FormRolPermission>();
    }
}
