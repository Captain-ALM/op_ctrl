# op_ctrl

This is operation control, now opensource!

Used for on machine control of other user accounts through a client program designed to be wrapped around another program that is run by the target user.
The server accepts connection from the clients, this is useful for RDP servers ;) . These clients can execute abitrary code from DLLs sent to them, don't worry, the connections are encrypted!

Provided Libraries:

- audiodll : Plays audio on the target!
- dumpdll : Dumps lsass!
- dump dll auto : Automatically dump lsass when the target is on (EG: Domain Administrator).
- msgbx_boot : Shows a message box for prompting the target.
- opdll : File system operations.

Why does the release have a login prompt (0.0.1.3)?

++ It's a long story but it stores the valid entered credentials in a serialized file next to the binary.

Why does the release have a dedication (0.0.1.3)?

++ I was heavily crushing on two people at the time (y10 anyone?) so its of no consequence.

Can I have a release without the login prompt?

++ Yes, 0.0.1.2 which is also provided with its source code.

What on earth was this used for originally?

++ No Comment.

Maintainer: 
Unmaintained now, sorry. (Was HCALM)

License: 
[BSD 3-Clause](https://github.com/Captain-ALM/op_ctrl/blob/master/LICENSE)
