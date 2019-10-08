using App.Data.Entities;
using MongoDB.Entities;
using System;
using System.Collections.Generic;

namespace App.Data.Managers
{
    public class EmailManager : ManagerBase<EmailMessage>
    {
        public List<EmailMessage> FetchNextBatch(int batchSize)
        {
            return DB.Find<EmailMessage>()
                     .Match(e => e.Sent == false)
                     .Sort(e => e.ModifiedOn, Order.Ascending)
                     .Limit(batchSize)
                     .Execute();
        }

        public void MarkAsSent(string id)
        {
            DB.Update<EmailMessage>()
              .Match(e => e.ID == id)
              .Modify(e => e.SentOn, DateTime.UtcNow)
              .Modify(e => e.Sent, true)
              .Execute();
        }

        public string GetTemplate(EmailTemplates template)
        {
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
                Mongo Web Api Starter</span>
              <br>
            </h1>
            <p style='font-family:Arial,Helvetica,sans-serif;font-size:small'>Thank you for creating an
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

            <p>Thank you for using Mongo Web Api Starter!</p>

            <p>--The Mongo Web Api Starter Team<br>
              <a href='https://website.com' target='_blank'>https://website.com</a></p>
            <p style='margin:3px auto;font:10px arial,sans-serif;color:#999'>© 2019 Copyright </p>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</body>
</html>
                        ";
                case EmailTemplates.Welcome_New_User:
                    return @"such empty";
            }

            return null;
        }
    }

    public enum EmailTemplates
    {
        Email_Address_Validation,
        Welcome_New_User
    }
}
