﻿using System;
using System.Collections.Generic;
using System.Linq;
using BootGen;
using Newtonsoft.Json.Linq;
using Pluralize.NET;

namespace WebProject.Generator
{
    public class JsonResourceCollection : ResourceCollection
    {
        public JsonResourceCollection(DataModel dataModel) : base(dataModel)
        {
        }

        public void Load(JObject jObject)
        {
            foreach (var property in jObject.Properties())
            {
                Parse(property);
            }
        }

        private ClassModel Parse(JProperty property)
        {
            var pluralizer = new Pluralizer();
            Noun className;
            if (property.Value.Type == JTokenType.Array)
            {
                className = pluralizer.Singularize(property.Name).Capitalize();
                className.Plural = property.Name.Capitalize();
            } else {
                className = property.Name.Capitalize();
                className.Plural = pluralizer.Pluralize(property.Name).Capitalize();
            }
            ClassModel result = DataModel.Classes.FirstOrDefault(c => c.Name.Singular == className);
            bool newClass = result == null;
            if (result == null)
            {
                result = new ClassModel
                {
                    Name = className,
                    Properties = new List<Property>()
                };
            }
            switch (property.Value.Type)
            {
                case JTokenType.Array:
                    var data = property.Value as JArray;
                    foreach (JObject item in data)
                    {
                        ExtendModel(result, item);
                    }
                break;
                case JTokenType.Object:
                    ExtendModel(result, property.Value as JObject);
                break;
                default:
                return null;
            }
            if (newClass) {
                DataModel.AddClass(result);
                Add(result);
            }
            return result;
        }

        private void ExtendModel(ClassModel model, JObject item)
        {
            foreach (var property in item.Properties())
            {
                var propertyName = property.Name.Capitalize();
                if (model.Properties.All(p => p.Name != propertyName))
                {
                    var prop = new Property
                    {
                        Name = propertyName,
                        BuiltInType = ConvertType(property.Value.Type),
                        IsCollection = property.Value.Type == JTokenType.Array
                    };
                    if (prop.BuiltInType == BuiltInType.Object)
                        prop.Class = Parse(property);
                    model.Properties.Add(prop);
                }
            }
        }

        private BuiltInType ConvertType(JTokenType type)
        {
            switch (type)
            {
                case JTokenType.Boolean:
                    return BuiltInType.Bool;
                case JTokenType.String:
                    return BuiltInType.String;
                case JTokenType.Date:
                    return BuiltInType.DateTime;
                case JTokenType.Integer:
                    return BuiltInType.Int32;
                case JTokenType.Guid:
                    return BuiltInType.Guid;
                case JTokenType.Float:
                    return BuiltInType.Float;
                case JTokenType.Array:
                    return BuiltInType.Object;
                case JTokenType.Object:
                    return BuiltInType.Object;
            }
            return BuiltInType.Object;
        }
    }
}
