using Xunit;

namespace UpToDocu {

    public class UTDObjectTest {
        [Fact]
        public void UTDObject_1_test() {
            var sut = new Test1Spec();

            Assert.Equal("", sut.SqlServer.Name);
            Assert.Equal("", sut.Database.Name);
            sut.Postpare();
            Assert.Equal(nameof(sut.SqlServer), sut.SqlServer.Name);
            Assert.Equal(nameof(sut.Database), sut.Database.Name);
        }

        public class Test1Spec : UtdSpecification {
            //public static Test1Spec Instance => GetInstance<Test1Spec>(()=>new Test1Spec());
            public static Test1Spec Instance => GetInstance(() => new Test1Spec());

            public Test1Spec() : base() {
            }

            public UtdObject SqlServer => Define(() => "MS SQL");

            public UtdObject Database => Define(() => SqlServer / "TodoDB");
        }
    }
}