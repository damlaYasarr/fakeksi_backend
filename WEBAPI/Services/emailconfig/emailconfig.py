import smtplib
# import the smtplib module. It should be included in Python by default

# set up the SMTP server _ after registration
s = smtplib.SMTP(host='your_host_address_here', port=your_port_here)
s.starttls()
s.login(MY_ADDRESS, PASSWORD)

def emailEntegration():
    pass
def get_contacts(filename):
    names = []
    emails = []
    with open(filename, mode='r', encoding='utf-8') as contacts_file:
        for a_contact in contacts_file:
            names.append(a_contact.split()[0])
            emails.append(a_contact.split()[1])
    return names, emails

