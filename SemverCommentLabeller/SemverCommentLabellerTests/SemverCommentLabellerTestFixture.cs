using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThoughtWorks.CruiseControl.Core;

namespace SemverCommentLabellerTests
{
    [TestClass]
    public class SemverCommentLabellerTestFixture
    {
        SemverCommentLabeller.SemverCommentLabeller _sut = new SemverCommentLabeller.SemverCommentLabeller();
        IntegrationResultStub _integrationResult = new IntegrationResultStub();

        [TestInitialize]
        public void Setup()
        {
        }

        //2.3.5.1239
        [TestMethod]
        public void TestNoPreviousBuild()
        {
            _integrationResult._last = null;
            _integrationResult.Modifications = new Modification[] { new Modification { Comment = "Major: a major change" } };
            string versionResult = _sut.Generate(_integrationResult);
            Assert.AreEqual("1.0.0.1240", versionResult);
        }

        [TestMethod]
        public void TestIncrementMajor()
        {
            _integrationResult.Modifications = new Modification[] { new Modification { Comment = "Major: a major change" } };
            string versionResult = _sut.Generate(_integrationResult);
            Assert.AreEqual("3.0.0.1240", versionResult);
        }

        [TestMethod]
        public void TestIncrementMinor()
        {
            _integrationResult.Modifications = new Modification[] { new Modification { Comment = "minor: a minor change" } };
            string versionResult = _sut.Generate(_integrationResult);
            Assert.AreEqual("2.4.0.1240", versionResult);
        }
        
        [TestMethod]
        public void TestIncrementPatch()
        {
            _integrationResult.Modifications = new Modification[] { new Modification { Comment = "patch: a bug fix" } };
            string versionResult = _sut.Generate(_integrationResult);
            Assert.AreEqual("2.3.6.1240", versionResult);
        }

        [TestMethod]
        public void TestIncrementRevisionOverflow()
        {
            _integrationResult._lastChangeNumber = "74444";

            _integrationResult.Modifications = new Modification[] { new Modification { Comment = "patch: a bug fix" } };
            string versionResult = _sut.Generate(_integrationResult);
            Assert.AreEqual("2.3.6.4444", versionResult);
        }
    }

    public class IntegrationResultStub : IIntegrationResult
    {
        public void AddTaskResult(ITaskResult result)
        {
            throw new NotImplementedException();
        }

        public void AddTaskResult(string result)
        {
            throw new NotImplementedException();
        }

        public string ArtifactDirectory
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string BaseFromArtifactsDirectory(string pathToBase)
        {
            throw new NotImplementedException();
        }

        public string BaseFromWorkingDirectory(string pathToBase)
        {
            throw new NotImplementedException();
        }

        public ThoughtWorks.CruiseControl.Remote.BuildCondition BuildCondition
        {
            get { throw new NotImplementedException(); }
        }

        public Guid BuildId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string BuildLogDirectory
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ThoughtWorks.CruiseControl.Core.Util.BuildProgressInformation BuildProgressInformation
        {
            get { throw new NotImplementedException(); }
        }

        public IIntegrationResult Clone()
        {
            throw new NotImplementedException();
        }

        public DateTime EndTime
        {
            get { throw new NotImplementedException(); }
        }

        public Exception ExceptionResult
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool Failed
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.ArrayList FailureTasks
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.ArrayList FailureUsers
        {
            get { throw new NotImplementedException(); }
        }

        public bool Fixed
        {
            get { throw new NotImplementedException(); }
        }

        public bool HasModifications()
        {
            return true;
        }

        public bool HasSourceControlError
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.IDictionary IntegrationProperties
        {
            get { throw new NotImplementedException(); }
        }

        public ThoughtWorks.CruiseControl.Remote.IntegrationRequest IntegrationRequest
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsInitial()
        {
            throw new NotImplementedException();
        }

        public string Label
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ThoughtWorks.CruiseControl.Remote.IntegrationStatus LastBuildStatus
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string _lastChangeNumber = "1240";
        public string LastChangeNumber
        {
            get { return _lastChangeNumber; }
        }

        public IntegrationSummary _last  = new IntegrationSummary(ThoughtWorks.CruiseControl.Remote.IntegrationStatus.Success, "2.3.5.1239", "2.3.5.1239", DateTime.Now.AddDays(-1));
        public IntegrationSummary LastIntegration
        {
            get {
                return _last;
            }
        }

        public ThoughtWorks.CruiseControl.Remote.IntegrationStatus LastIntegrationStatus
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime LastModificationDate
        {
            get { throw new NotImplementedException(); }
        }

        public string LastSuccessfulIntegrationLabel
        {
            get { throw new NotImplementedException(); }
        }

        public void MarkEndTime()
        {
            throw new NotImplementedException();
        }

        public void MarkStartTime()
        {
            throw new NotImplementedException();
        }

        public void Merge(IIntegrationResult value)
        {
            throw new NotImplementedException();
        }

        public Modification[] Modifications { get; set; }

        public System.Collections.Generic.List<ThoughtWorks.CruiseControl.Remote.NameValuePair> Parameters
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ProjectName
        {
            get { throw new NotImplementedException(); }
        }

        public string ProjectUrl
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool ShouldRunBuild()
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.List<ThoughtWorks.CruiseControl.Remote.NameValuePair> SourceControlData
        {
            get { throw new NotImplementedException(); }
        }

        public Exception SourceControlError
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime StartTime
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ThoughtWorks.CruiseControl.Remote.IntegrationStatus Status
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool Succeeded
        {
            get { throw new NotImplementedException(); }
        }

        public string TaskOutput
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.IList TaskResults
        {
            get { throw new NotImplementedException(); }
        }

        public TimeSpan TotalIntegrationTime
        {
            get { throw new NotImplementedException(); }
        }

        public string WorkingDirectory
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }

}
