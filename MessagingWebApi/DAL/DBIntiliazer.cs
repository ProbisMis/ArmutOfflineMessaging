//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using MessagingWebApi.Data;
//using MessagingWebApi.Models;


//namespace MessagingWebApi.DAL
//{
//   public class DBInitiliazer : System.Data.Entity. DropCreateDatabaseIfModelChanges<MessagingWebApiContext>
//    {
//        protected override void Seed(MessagingWebApiContext context)
//        {
//            var students = new List<User>
//            {
//            new User{Username="Carson",Password="Alexander",CreatedDate=DateTime.Parse("2005-09-01")},
//            new User{Username="Meredith",Password="Alonso",CreatedDate=DateTime.Parse("2002-09-01")},
//            new User{Username="Arturo",Password="Anand",CreatedDate=DateTime.Parse("2003-09-01")},
//            new User{Username="Gytis",Password="Barzdukas",CreatedDate=DateTime.Parse("2002-09-01")},
//            new User{Username="Yan",Password="Li",CreatedDate=DateTime.Parse("2002-09-01")},
//            new User{Username="Peggy",Password="Justice",CreatedDate=DateTime.Parse("2001-09-01")},
//            new User{Username="Laura",Password="Norman",CreatedDate=DateTime.Parse("2003-09-01")},
//            new User{Username="Nino",Password="Olivetto",CreatedDate=DateTime.Parse("2005-09-01")}
//            };

//            students.ForEach(s => context.Users.Add(s));
//            context.SaveChanges();
//            var courses = new List<Chat>
//            {
//            new Chat{ChatGuid = "YanPeggy",CreatedDate=DateTime.Parse("2005-09-01"), SenderId = 5, RecieverId = 6},
//            new Chat{ChatGuid = "CarsonMeredith",CreatedDate=DateTime.Parse("2005-09-01"), SenderId = 1, RecieverId = 2 },
            
//            };
//            courses.ForEach(s => context.Chats.Add(s));
//            context.SaveChanges();
//            var enrollments = new List<Message>
//            {
//            new Message{SenderId=1,RecieverId=2, CreatedDate=DateTime.Parse("2005-09-01"), Body = "H1"},
//            new Message{SenderId=1,RecieverId=2,CreatedDate=DateTime.Parse("2005-09-01"), Body = "H2"},
//            new Message{SenderId=1,RecieverId=2,CreatedDate=DateTime.Parse("2005-09-01"), Body = "H3"},
//            new Message{SenderId=2,RecieverId=1,CreatedDate=DateTime.Parse("2005-09-01"), Body = "H4"},
//            new Message{SenderId=2,RecieverId=1,CreatedDate=DateTime.Parse("2005-09-01"), Body = "H5"},
         
//            };
//            enrollments.ForEach(s => context.Messages.Add(s));
//            context.SaveChanges();
//        }
//    }
//}
