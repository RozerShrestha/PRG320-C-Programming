namespace ConsoleProjectUnitTest
{
    public class Tests
    {
        private Student student;

        [SetUp]
        public void Setup()
        {
            // Arrange common test data
            student = new Student
            {
                Name = "Test Student",
                Grades = new List<double> { 60, 70, 80 }
            };

        }

        [Test]
        public void CalculateAverage_ReturnsAverageFromGivenNumbersInList()
        {
           
            //Act
            double average = student.CalculateAverage();
            //Assert
            Assert.That(average, Is.EqualTo(70));
        }

        [Test]
        public void CalculateAverage_ReturnsAverageFromGivenNumbers()
        {

            //Act
            double average = student.CalculateAverage();
            //Assert
            Assert.AreNotEqual(average, 80);
        }
    }
}