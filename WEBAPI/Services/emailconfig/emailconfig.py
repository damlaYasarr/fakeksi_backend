#!/usr/bin/python
import smtplib
from email.message import EmailMessage
import ssl 

email_sender='dsadadad12@gmail.com'
email_password='slppzvmxttipdnxo' # two step google verification

def main(email_receiver):
    subject="fakeksi email verificaton"
    body="click the link and go to login page  http://localhost:4200/(bla:home/login)"

    em=EmailMessage()
    em['From']=email_sender
    em['To']=email_receiver
    em['subject']=subject
    em.set_content(body) 
 

    context = ssl.create_default_context()
    with smtplib.SMTP_SSL('smtp.googlemail.com', 465,  context=context, local_hostname="localhost:7095") as smtp:
        smtp.login(email_sender,email_password)
        smtp.sendmail(email_sender,email_receiver,em.as_string())
   

