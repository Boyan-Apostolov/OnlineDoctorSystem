namespace OnlineDoctorSystem.Services.Data.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Web.ViewModels.Prescriptions;
    using Xunit;

    public class PrescriptionsServiceTests : BaseTestClass
    {
        [Fact]
        public async Task CreatingPrescriptionShouldAddItToTheDb()
        {
            var prescription = new AddPrescriptionInputModel()
            {
                Doctor = new Doctor(),
                DoctorId = "test",
                Patient = new Patient(),
                PatientId = "test",
                Instructions = "Test",
                MedicamentName = "Test",
                Quantity = "Test123",
            };
            await this.PrescriptionsService.AddPrescriptionToPatient(prescription);

            var prescriptionFromService = this.PrescribtionsRepository.All().First();

            Assert.Equal(prescription.Quantity, prescriptionFromService.Quantity);
        }

        [Fact]
        public async Task GettingPatientsPrescriptionsShouldReturnTheCorrectNumberOfPrescriptions()
        {
            var user1 = new ApplicationUser() { UserName = "test", Email = "test@test.com" };
            var user2 = new ApplicationUser() { UserName = "test2", Email = "test2@test.com" };

            var patient = new Patient()
            {
                LastName = "Test",
                UserId = user1.Id,
                User = user1,
            };
            await this.PatientsRepository.AddAsync(patient);

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            await this.DoctorsRepository.AddAsync(doctor);
            var prescription = new AddPrescriptionInputModel()
            {
                Doctor = doctor,
                DoctorId = doctor.Id,
                Patient = patient,
                PatientId = patient.Id,
                Instructions = "Test",
                MedicamentName = "Test",
                Quantity = "Test123",
            };
            await this.PrescriptionsService.AddPrescriptionToPatient(prescription);

            var prescriptions =
                this.PrescriptionsService.GetPatientsPrescriptions<AddPrescriptionInputModel>(patient.Id);

            var prescriptionsCount = prescriptions.Count();

            Assert.Equal(1, prescriptionsCount);
        }
    }
}
