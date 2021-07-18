using NUnit.Framework;
using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using Moq;

namespace TestKanban
{
    public class Tests
    {
        Mock<PasswordValidetorMock> PVM;
        Mock<UserContollerMock> UCM;
        Mock<UserMock> UM;

        [SetUp]
        public void Setup()
        {

            PVM = new Mock<PasswordValidetorMock>();
            UCM = new Mock<UserContollerMock>();
            UM = new Mock<UserMock>();
        }

        [Test]
        public void Register_succeed()
        {
            // arrange
            UserController UC = new();
            PVM.Setup(m => m.ValidPassword()).Returns(true);
            UCM.Setup(m => m.IsValidEmail("GALMOR@gmail.com")).Returns(true);

            // act
            UC.RegisterMock("GALMOR@gmail.com", "A231" , PVM.Object);

            // assert
            Assert.AreNotEqual(UC.userDict["GALMOR@gmail.com"], null);
        }
        [Test]
        public void Register_Failed_NullEmail()
        {
            // arrange
            UserController UC = new();
            PVM.Setup(m => m.ValidPassword()).Returns(true);
            UCM.Setup(m => m.IsValidEmail("GALMOR@gmail.com")).Returns(true);


            // act
            try { 
                UC.RegisterMock(null, "A231", PVM.Object);
                Assert.Fail();
            }
            // assert
            catch (Exception e)
            {
                Assert.Pass();
            }

        }
        [Test]
        public void Register_Failed_NullPassword()
        {
            // arrange
            UserController UC = new();
            PVM.Setup(m => m.ValidPassword()).Returns(true);
            UCM.Setup(m => m.IsValidEmail("GALMOR@gmail.com")).Returns(true);


            // act
            try
            {
                UC.RegisterMock("GALMOR@gmail.com", null, PVM.Object); ;
                Assert.Fail();
            }
            // assert
            catch (Exception e)
            {
                Assert.Pass();
            }

        }
        [Test]
        public void Register_Failed_Not_Valid_Email()
        {
            // arrange
            UserController UC = new();
            PVM.Setup(m => m.ValidPassword()).Returns(true);
            UCM.Setup(m => m.IsValidEmail("GALMOR@gmail.com")).Returns(true);


            // act
            try
            {
                UC.RegisterMock("GALMOR@gmail.com", "Aa123", PVM.Object); ;
                //Assert.Fail();
            }
            // assert
            catch (Exception e)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void LogIn_succeed()
        {
          
            // arrange
            UserController UC = new();
            UC.userDict.Add("GALMOR@gmail.com", new User("GALMOR@gmail.com", "Aa12345678",0));
            string PASS = "Aa12345678";
            UM.Setup(m => m.ConfirmPassword(PASS)).Returns(true);

            // act
            UC.LogIn("GALMOR@gmail.com", "Aa12345678");
            // assert
            Assert.True(UC.userDict["GALMOR@gmail.com"].LoggedIn);
        }
        [Test]
        public void LogIn_Failed_NullEmail()
        {
            // arrange
            UserController UC = new();
            UC.userDict.Add("GALMOR@gmail.com", new User("GALMOR@gmail.com", "Aa12345678", 0));


            // act
            try
            {
                UC.LogIn(null, "Aa12345678");
                Assert.Fail();
            }
            // assert
            catch (Exception e)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void LogIn_Failed_NullPassword()
        {
            // arrange
            UserController UC = new();
            UC.userDict.Add("GALMOR@gmail.com", new User("GALMOR@gmail.com", "Aa12345678", 0));


            // act
            try
            {
                UC.LogIn("GALMOR@gmail.com", null);
                Assert.Fail();
            }
            // assert
            catch (Exception e)
            {
                Assert.Pass();
            }

        }
        [Test]
        public void LogIn_Failed_Not_In_Collection()
        {
            // arrange
            UserController UC = new();


            // act
            try
            {
                UC.LogIn("GALMOR@gmail.com", "Aa12345678");
                Assert.Fail();
            }
            // assert
            catch (Exception e)
            {
                Assert.Pass();
            }

        }
        [Test]
        public void LogIn__Failed_Already_Login()
        {
			// arrange
			UserController UC = new();
            UC.userDict.Add("GALMOR@gmail.com", new User("GALMOR@gmail.com", "Aa12345678", 0));
            string PASS = "Aa12345678";
            UM.Setup(m => m.ConfirmPassword(PASS)).Returns(true);
            UC.LogIn("GALMOR@gmail.com", "Aa12345678");
            // act
            try
            {
                UC.LogIn("GALMOR@gmail.com", "Aa12345678");
                Assert.Fail();
            }
            // assert
            catch (Exception e)
            {
                Assert.Pass();
            }
        }
        [Test]
        public void Login__Failed_WrongPassword()
        {
            // arrange
            UserController UC = new();
            UC.userDict.Add("GALMOR@gmail.com", new User("GALMOR@gmail.com", "Aa12345678", 0));
            // act
            try
            {
                UC.LogIn("GALMOR@gmail.com", "Aa123456789");
                Assert.Fail();
            }
            // assert
            catch (Exception e)
            {
                Assert.Pass();
            }
        }
        [Test]
        public void validLogin_succeed()
        {
            // arrange
            UserController UC = new();
            UC.userDict.Add("GALMOR@gmail.com", new User("GALMOR@gmail.com", "Aa12345678", 0));
            UC.LogIn("GALMOR@gmail.com", "Aa12345678");
            // act
            bool c = UC.validLogin("GALMOR@gmail.com");
            // assert
            Assert.True(c);

        }
        [Test]
        public void validLogin_succeed_NotLogin()
        {
            // arrange
            UserController UC = new();
            UC.userDict.Add("GALMOR@gmail.com", new User("GALMOR@gmail.com", "Aa12345678", 0));
            // act
            bool c = UC.validLogin("GALMOR@gmail.com");
            // assert
            Assert.False(c);

        }
        [Test]
        public void validLogin__Failed_NullEmail()
        {
            // arrange
            UserController UC = new();
            UC.userDict.Add("GALMOR@gmail.com", new User("GALMOR@gmail.com", "Aa12345678", 0));
            UC.LogIn("GALMOR@gmail.com", "Aa12345678");

            // act
            try
            {
                bool c = UC.validLogin(null);
                Assert.Fail();
            }
            // assert
            catch (Exception e)
            {
                Assert.Pass();
            }
            

        }
        [Test]
        public void validLogin__Failed_EmilNotInCollection()
        {
            UserController UC = new();
            UC.userDict.Add("GALMOR@gmail.com", new User("GALMOR@gmail.com", "Aa12345678", 0));
            try
            {
                bool c = UC.validLogin("GALMOR@gmail.com");
                Assert.Fail();
            }
            // assert
            catch (Exception e)
            {
                Assert.Pass();
            }
        }


    }

}