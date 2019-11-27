using LighteningBolt.Application.AppServices.Test.Dto;
using LighteningBolt.Core.Data.Repo;
using LighteningBolt.Core.Entities;
using LighteningBolt.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LighteningBolt.Application.AppServices.TestParticipants
{
    public class TestParticipantsAppServices : ITestParticipantsAppServices
    {
        private readonly IRepository<TestParticipant> _athleteTestParticipantsRepo;
        private readonly ApplicationDbContext _context;
        private CreateAthleteTestParticipant _createAthleteTestParticipant;

        public TestParticipantsAppServices(IRepository<TestParticipant> athleteTestParticipantsRepo, ApplicationDbContext context)
        {
            _athleteTestParticipantsRepo = athleteTestParticipantsRepo;
            _context = context;
        }

        public void CreateAtheleTestParticipant(CreateAthleteTestParticipant input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            //Apply Business Validation(if any)

            //Mapping 
            var athletTestParticipant = new TestParticipant
            {
                AddedDate = input.AddedDate,
                ModifiedDate = input.ModifiedDate,
                UserId = input.UserId,
                CreatedBy = input.CreatedBy,
                DistanceCovered = input.DistanceCovered,
                TimeTaken = input.TimeTaken,
                AthleteTest = input.AthleteTest
            };

            _athleteTestParticipantsRepo.Insert(athletTestParticipant);
        }

        public bool DeleteAthleteTestParticipant(long id)
        {
            bool isSuccess = false;
            try
            {
                var recordDelete = _athleteTestParticipantsRepo.Get(id);
                _athleteTestParticipantsRepo.Delete(recordDelete);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public List<CreateAthleteTestParticipant> FetchAthleteTestParticipants(long testId)
        {
            var userList = _context.Users.ToList();
            return _athleteTestParticipantsRepo.GetAll().Where(x => x.AthleteTest.Id == testId || testId == 0).Select(rec => new CreateAthleteTestParticipant()
            {
                Id = rec.Id,
                AthleteTest = rec.AthleteTest,
                AddedDate = rec.AddedDate,
                UserId = rec.UserId,
                CreatedBy = rec.CreatedBy,
                DistanceCovered = rec.DistanceCovered,
                ModifiedDate = rec.ModifiedDate,
                TimeTaken = rec.TimeTaken,
                UserName = userList.Where(x => Guid.Parse(x.Id) == rec.UserId).Select(nam => nam.UserName).SingleOrDefault()

            }).ToList();
        }

        public CreateAthleteTestParticipant FetchTestParticipantById(long id)
        {           

            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            //Apply Business Validation(if any)

            //Mapping

            var athleteTestParticipant = _athleteTestParticipantsRepo.Get(id);
            if (athleteTestParticipant != default(TestParticipant))
            {
                _createAthleteTestParticipant = new CreateAthleteTestParticipant()
                {
                    Id = athleteTestParticipant.Id,
                    UserId = athleteTestParticipant.UserId,
                    AddedDate = athleteTestParticipant.AddedDate,
                    ModifiedDate = athleteTestParticipant.ModifiedDate,
                    AthleteTest = athleteTestParticipant.AthleteTest,
                    CreatedBy = athleteTestParticipant.CreatedBy,
                    DistanceCovered = athleteTestParticipant.DistanceCovered,
                    TimeTaken = athleteTestParticipant.TimeTaken
                };
            }
            else
            {
                _createAthleteTestParticipant = new CreateAthleteTestParticipant();
            }

            return _createAthleteTestParticipant;

        }


        public TestParticipant FetchTestParticipantByUserId(Guid userId, long testId)
        {            

            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            //Apply Business Validation(if any)

            //Mapping

            var athleteTestParticipant = _athleteTestParticipantsRepo.GetAll().Where(x => x.UserId == userId && x.AthleteTest.Id == testId).FirstOrDefault();
            return athleteTestParticipant;

        }

        public void UpdateAtheleTestParticipant(TestParticipant testParticipant)
        {

            if (testParticipant == null)
            {
                throw new ArgumentNullException(nameof(testParticipant));
            }

            //Apply Business Validation(if any)

            //Mapping

            _athleteTestParticipantsRepo.Update(testParticipant);
            _context.SaveChanges();



        }
    }
}
