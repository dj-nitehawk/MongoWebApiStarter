using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoWebApiStarter.Biz;

namespace MongoWebApiStarter.Test
{
    [TestClass]
    public class MapLocationTest
    {
        [TestMethod]
        public void blank_link_passes_as_valid()
        {
            MapLocation.IsValid("").Should().BeTrue();
        }

        [TestMethod]
        public void invalid_link_should_fail()
        {
            MapLocation.IsValid("jkheglkjrhgklrejh").Should().BeFalse();
        }

        [TestMethod]
        public void valid_link_passes()
        {
            MapLocation.IsValid("https://www.google.com/maps/place/Paris,+France/@48.8589507,2.2770204,12z/").Should().BeTrue();
        }

        [TestMethod]
        public void returns_correct_lat_long_values()
        {
            var res = MapLocation.Get2DCoordinates("https://www.google.com/maps/place/Paris,+France/@48.8589507,2.2770204,12z/");

            res.coordinates[0].Should().Be(2.2770204);
            res.coordinates[1].Should().Be(48.8589507);
            res.type.Should().Be("Point");
        }

        [TestMethod]
        public void returns_correct_lat_long_values_for_embed_code()
        {
            var res = MapLocation.Get2DCoordinates(@"https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d507004.95965405094!2d79.9199045!3d6.8934444!3m2!1i1024!2i768!4f13.1!
3m3!1m2!1s0x3ae259f2520d603d%3A0xbb7ca7275b1ce75e!2sDomino&#39;s%20Pizza!5e0!3m2!1sen!2slk!4v1579960075834!5m2!1sen!2slk");

            res.coordinates[0].Should().Be(79.9199045);
            res.coordinates[1].Should().Be(6.8934444);
            res.type.Should().Be("Point");
        }
    }
}
