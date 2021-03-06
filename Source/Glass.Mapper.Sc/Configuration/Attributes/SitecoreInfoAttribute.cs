/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    /// <summary>
    /// Class SitecoreInfoAttribute
    /// </summary>
    public class SitecoreInfoAttribute : InfoAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreInfoAttribute"/> class.
        /// </summary>
        public SitecoreInfoAttribute()
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreInfoAttribute"/> class.
        /// </summary>
        /// <param name="infoType">Type of the info.</param>
        public SitecoreInfoAttribute(SitecoreInfoType infoType)
        {
            Type = infoType;
        }

        /// <summary>
        /// The type of information that should populate the property
        /// </summary>
        /// <value>The type.</value>
        public SitecoreInfoType Type { get; set; }

        /// <summary>
        /// UrlOptions, use in conjunction with SitecoreInfoType.Url
        /// </summary>
        /// <value>The URL options.</value>
        public SitecoreInfoUrlOptions UrlOptions { get; set; }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>AbstractPropertyConfiguration.</returns>
        public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
        {
            var config = new SitecoreInfoConfiguration();
            Configure(propertyInfo, config);
            return config;
        }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public void Configure(PropertyInfo propertyInfo, SitecoreInfoConfiguration config)
        {
            config.Type = this.Type;
            config.UrlOptions = this.UrlOptions;

            base.Configure(propertyInfo, config);
        }
    }
}



