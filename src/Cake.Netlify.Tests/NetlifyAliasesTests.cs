using System;
using Cake.Core;
using Cake.Netlify.Tests.Fixture;
using NSubstitute;
using Should;
using Xunit;

namespace Cake.Netlify.Tests {
    public sealed class NetlifyAliasesTests {
        [Fact]
        public void Should_Throw_If_Context_Is_Null() {
            // Given
            var fixture = new NetlifyDeployFixture();

            // When
            var result = Record.Exception(() => NetlifyAliases.NetlifyDeploy(
                null, fixture.DirectoryToDeploy, fixture.Settings));

            // Then
            result.ShouldBeType<ArgumentNullException>().ParamName.ShouldEqual("context");
        }

        [Fact]
        public void Should_Throw_If_Directory_To_Deploy_Is_Null() {
            // Given
            var fixture = new NetlifyDeployFixture();
            var context = Substitute.For<ICakeContext>();

            // When
            var result = Record.Exception(() => NetlifyAliases.NetlifyDeploy(
                context, null, fixture.Settings));
            // Then
            result.ShouldBeType<ArgumentNullException>().ParamName.ShouldEqual("directoryToDeploy");
        }

        [Fact]
        public void Should_Throw_If_Site_Id_Is_Null() {
            // Given
            var context = Substitute.For<ICakeContext>();

            // When
            var result = Record.Exception(() => NetlifyAliases.NetlifyDeploy(
                context, null, "my-token"));
            // Then
            result.ShouldBeType<ArgumentNullException>().ParamName.ShouldEqual("siteId");
        }

        [Fact]
        public void Should_Throw_If_Token_Is_Null() {
            // Given
            var context = Substitute.For<ICakeContext>();

            // When
            var result = Record.Exception(() => NetlifyAliases.NetlifyDeploy(
                context, "my-site-id", null));
            // Then
            result.ShouldBeType<ArgumentNullException>().ParamName.ShouldEqual("token");
        }
    }
}