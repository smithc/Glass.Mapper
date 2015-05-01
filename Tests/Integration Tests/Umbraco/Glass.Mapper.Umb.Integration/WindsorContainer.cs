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
using System.Text;
using Glass.Mapper.Umb.CastleWindsor;

namespace Glass.Mapper.Umb.Integration
{
    class WindsorContainer
    {
        public static Context GetContext()
        {
            var config = new Glass.Mapper.Umb.CastleWindsor.Config();
            var resolver = DependencyResolver.CreateStandardResolver();
            var context = Context.Create(resolver);
            ((DependencyResolver)resolver).Container.Install(new UmbracoInstaller(config));
            return context;
        }
    }
}

