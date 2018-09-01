﻿using System;
using Cake.Core;
using Cake.Netlify.Tests.Fixture;
using Cake.Testing;
using FluentAssertions;
using Xunit;

namespace Cake.Netlify.Tests {
    public sealed class NetlifyDeployRunnerTests {
        [Fact]
        public void Should_Throw_If_Settings_Are_Null() {
            // Given
            var fixture = new NetlifyDeployFixture();
            fixture.Settings = null;

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            result.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Equals("settings");
        }

        [Fact]
        public void Should_Throw_If_Directory_To_Deploy_Is_Null() {
            // Given
            var fixture = new NetlifyDeployFixture();
            fixture.DirectoryToDeploy = null;

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            result.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Equals("directoryToDeploy");
        }

        [Fact]
        public void Should_Throw_If_Netlify_Executable_Was_Not_Found() {
            // Given
            var fixture = new NetlifyDeployFixture();
            fixture.GivenDefaultToolDoNotExist();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            result.Should().BeOfType<CakeException>().Subject.Message.Should().Equals("Netlify: Could not locate executable.");
        }

        [Theory]
        [InlineData("/bin/tools/Netlify/netlify.cmd", "/bin/tools/Netlify/netlify.cmd")]
        [InlineData("./tools/Netlify/netlify.cmd", "/Working/tools/Netlify/netlify.cmd")]
        public void Should_Use_Netlify_Executable_From_Tool_Path_If_Provided(string toolPath, string expected) {
            // Given
            var fixture = new NetlifyDeployFixture();
            fixture.Settings.ToolPath = toolPath;
            fixture.GivenSettingsToolPathExist();

            // When
            var result = fixture.Run();

            // Then
            result.Path.FullPath.Should().Equals(expected);
        }

        [Fact]
        public void Should_Throw_If_Process_Was_Not_Started() {
            // Given
            var fixture = new NetlifyDeployFixture();
            fixture.GivenProcessCannotStart();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            result.Should().BeOfType<CakeException>().Subject.Message.Should().Equals("Netlify: Process was not started.");
        }

        [Fact]
        public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code() {
            // Given
            var fixture = new NetlifyDeployFixture();
            fixture.GivenProcessExitsWithCode(1);

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            result.Should().BeOfType<CakeException>().Subject.Message
                .Should().Equals("Netlify: Process returned an error (exit code 1).");
        }

        [Fact]
        public void Should_Find_Netlify_Executable_If_Tool_Path_Not_Provided() {
            // Given
            var fixture = new NetlifyDeployFixture();

            // When
            var result = fixture.Run();

            // Then
            result.Path.FullPath.Should().Equals("/Working/tools/netlify.cmd");
        }

        [Fact]
        public void Should_Add_Directory_To_Deploy_To_Arguments() {
            // Given 
            var fixture = new NetlifyDeployFixture();

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Equals("deploy -p \"/Working/dist\"");
        }

        [Fact]
        public void Should_Add_Site_Id_To_Arguments() {
            // Given 
            var fixture = new NetlifyDeployFixture();
            fixture.Settings.SiteId = "123";

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Equals("deploy -p \"/Working/dist\" -s 123");
        }

        [Fact]
        public void Should_Add_Token_To_Arguments() {
            // Given 
            var fixture = new NetlifyDeployFixture();
            fixture.Settings.SiteId = "123";
            fixture.Settings.Token = "456";
            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Equals("deploy -p \"/Working/dist\" -s 123 -t 456");
        }

        [Fact]
        public void Should_Add_Draft_To_Arguments() {
            // Given 
            var fixture = new NetlifyDeployFixture();
            fixture.Settings.SiteId = "123";
            fixture.Settings.Token = "456";
            fixture.Settings.Draft = true;

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Equals("deploy -p \"/Working/dist\" -s 123 -t 456 -d");
        }

        [Fact]
        public void Should_Add_Environment_To_Arguments() {
            // Given 
            var fixture = new NetlifyDeployFixture();
            fixture.Settings.SiteId = "123";
            fixture.Settings.Token = "456";
            fixture.Settings.Draft = true;
            fixture.Settings.Environment = "production";

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Equals("deploy -p \"/Working/dist\" -s 123 -t 456 -d -e production");
        }
    }
}