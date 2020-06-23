using MongoDB.Entities;
using System;
using System.Collections.Generic;

namespace Data
{
    public class RepoEmail : RepoBase<EmailMessage>
    {
        static RepoEmail()
        {
            DB.Index<EmailMessage>()
              .Key(e => e.Sent, KeyType.Ascending)
              .CreateAsync();
        }

        public static List<EmailMessage> FetchNextBatch(int batchSize)
        {
            return DB.Find<EmailMessage>()
                     .Match(e => e.Sent != true)
                     .Sort(e => e.ModifiedOn, Order.Ascending)
                     .Limit(batchSize)
                     .Execute();
        }

        public static void MarkAsSent(string id)
        {
            DB.Update<EmailMessage>()
              .Match(e => e.ID == id)
              .Modify(e => e.SentOn, DateTime.UtcNow)
              .Modify(e => e.Sent, true)
              .Execute();
        }

        public static string GetTemplate(EmailTemplates template)
        {
            //fetch from db after they can be edited from admin backend.
            switch (template)
            {
                case EmailTemplates.Email_Address_Validation:
                    return
                        @"
<html>
<body>
  <div>
    <table border='0' width='550' cellpadding='0' cellspacing='0'
      style='max-width:550px;border-top:4px solid #39c;font:12px arial,sans-serif;margin:0 auto'>
      <tbody>
        <tr>
          <td>
            <h1 style='color:#000;font:bold 23px arial;margin:5px 0'>
              <span
                style='color:rgb(74,176,156);font-family:-apple-system,BlinkMacSystemFont,Segoe UI,Roboto,Oxygen-Sans,Ubuntu,Cantarell,Helvetica Neue,sans-serif;font-size:35px;font-weight:400'>
                Shared Care Vault</span>
              <br>
            </h1>
            <p style='font-family:Arial,Helvetica,sans-serif;font-size:small'>Thank you for creating a Virtual Practice
              account on our system.<br></p>
            <p style='font-family:Arial,Helvetica,sans-serif;font-size:small'><strong
                style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;font-size:12px'><a
                  href='{ValidationLink}' target='_blank'>Click here</a></strong><span
                style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;font-size:12px'> to confirm your email
                address.</span><br></p>

            <p>If the above link does not work, you can paste the following address into your browser:</p>

            <p>{ValidationLink}</p>

            <p>This is the address you will log in with, and the address to which we will deliver all email messages and
              other system mail.</p>

            <p>Thank you for using Shared Care Vault!</p>

            <p>--The MongoWebApiStarter Team<br>
              <a href='https://MongoWebApiStarter.com' target='_blank'>https://MongoWebApiStarter.com</a></p>
            <p style='margin:3px auto;font:10px arial,sans-serif;color:#999'>© 2019 Copyright </p>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</body>
</html>
                        ";
                case EmailTemplates.New_Employee_Invitation:
                    return
                        @"
<html>
<body>
  <div>
    <p>Dear {Salutation}</p>
    <p>You have been added as an employee of {VPName} with the following roles:</p>
    {RoleList}
    <p>In order to complete the sign-up process and create a password, please click the link below:</p>
    <a href='{SignUpLink}'>Click here to continue!</a>
    <p>Thank you!</p>
  </div>
</body>
</html>
                        ";

                case EmailTemplates.Welcome_New_VirtualPractice:
                    return "";
            }

            return null;
        }
    }

    public enum EmailTemplates
    {
        Email_Address_Validation = 0,
        New_Employee_Invitation = 1,
        Welcome_New_VirtualPractice = 2
    }
}
