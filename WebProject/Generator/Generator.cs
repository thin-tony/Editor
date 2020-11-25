
using System.Collections.Generic;
using BootGen;

namespace WebProject.Generator
{
    public class Generator
    {
        private Api Api;
        private AspNetCoreGenerator Gen;

        public Generator(JsonResourceCollection collection, string nameSpace)
        {
            Api = new Api(collection);
            Gen = new AspNetCoreGenerator("");
            Gen.NameSpace = nameSpace;
        }
        public IEnumerable<GeneratedFile> GenerateClasses()
        {
            int idx = 0;
            foreach ( var content in Gen.RenderClasses("templates/csharp_model.sbn", Api.DataModel.Classes))
            {
                yield return new GeneratedFile {
                    Name = $"{Api.DataModel.Classes[idx].Name}.cs",
                    Path = "models",
                    Content = content
                };
                idx += 1;
            }
        }

        public GeneratedFile GenerateDBContext(SeedDataStore seedStore)
        {
            return new GeneratedFile {
                Name = "DataContext.cs",
                Path = "",
                Content = Gen.Render("templates/dataContext.sbn", new Dictionary<string, object> {
                {"classes", Api.DataModel.StoredClasses},
                {"seedList", seedStore.All()},
                {"name_space", Gen.NameSpace}
              })
            };
        }
    }
}