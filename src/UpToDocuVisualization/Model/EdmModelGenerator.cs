using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace UpToDocuVisualization.Model {
    public class EdmModelGenerator {
        public static IEdmModel GetEdmModel() {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.Namespace = "UpToDocu";

            /*
            var entitySetUser = odataBuilder.EntitySet<UserModel>("User");
            var entityTypeUser = entitySetUser.EntityType;
            entityTypeUser.Property(user => user.UserId);
            entityTypeUser.Property(user => user.DisplayName);
            entityTypeUser.Property(user => user.ValidFrom);
            entityTypeUser.Property(user => user.ValidTo);
            entityTypeUser.HasKey(user => user.UserId);
            */

            return odataBuilder.GetEdmModel();
        }
    }
}
