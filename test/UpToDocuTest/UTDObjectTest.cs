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

        public class Test1Spec : UTDSpecification {
            public Test1Spec() : base() {
            }

            public UTDObject SqlServer => Define(() => "MS SQL");

            public UTDObject Database => Define(() => SqlServer / "TodoDB");
        }
    }
}