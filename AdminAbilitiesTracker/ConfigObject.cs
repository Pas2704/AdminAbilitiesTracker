﻿using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdminAbilitiesTracker
{
    [XmlRoot("AdminAbilitiesTrackerConfig")]
    public class ConfigObject
    {
        public bool SendChatMessages;
    }
}
