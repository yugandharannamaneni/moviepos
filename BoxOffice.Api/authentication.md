# Group Authentication

The following section describes the basic concepts of **Authentication** and **Authorization** used by and required for communicating with the API.

## UserToken and SessionToken [/UserTokenSessionToken]
A UserToken is used to identify a particular bwin.party customer. Depending on the called service it may be required or not. In general, all customer specific services like Account Data need valid tokens while static data services like the country list don’t.

User- and SessionToken are obtained through a successful [Login](#authentication-login). The returned tokens should be stored by the client throughout the whole session and shall be sent with every PosAPI request that is related to this customer as HTTP headers or URI-parameters:

```http
x-bwin-session-token: [SessionToken]
x-bwin-user-token: [UserToken]
```

The UserToken is valid for 20 hours after session start. The SessionToken is valid for 20 minutes after the last request  (10 minutes for Italy). In case the token has to be renewed without triggering any other action on PosAPI side the Refresh method can be used.

All passed Tokens are validated on each incoming requests, and in case of violation dedicated ErroCodes are returned.

| Status Code | Error Code | Message                                  |
|:-----------:|:----------:|------------------------------------------|
| `400`       | `201`      | Invalid security token (deprecated)      |
| `400`       | `204`      | Missing access token                     |
| `400`       | `205`      | Missing session token                    |
| `400`       | `206`      | Missing user token                       |
| `400`       | `207`      | Session security token expired           |
| `400`       | `208`      | User security token expired              |
| `400`       | `209`      | Session token invalid                    |
| `400`       | `210`      | User token invalid                       |


## Account Category Ids [/AccountCategoryIds]

| ID   | Description                                           |
|:----:|-------------------------------------------------------|
| `-1` | Unknown account category or category id not supported |
| `0`  | New user                                              |
| `1`  | User is RGclosed                                      |
| `2`  | User is in a closed state with CoolOff                |
| `3`  | User is suspended                                     |
| `4`  | User is closed                                        |


## Player Category Ids [/PlayerCategoryIds]

| ID  | Description |
|:---:|-------------|
| `0` | Play user   |
| `1` | Real user   |
| `2` | Pseudo user |


## CheckCountryBlock [/Authentication.svc/CheckCountryBlock?countryId={id}]
PosAPI holds a simple rule-set to provide information about country-specific actions, e.g. redirection to a targetUrl, displaying content, and so on. The ruleset takes under consideration the used AccessId (frontendID, brandID, … parameters) and the twoletter countryCode. If no matching rule exists it will return with an HTTP 200, otherwise the response will be an HTTP 403 Forbidden, containing a keyValue-list of additional parameters.

### Checks if the given country is blocked [GET]

+ Parameters

    + id (required, string, `CH`) ... The twoletter countrycode of the country. [ISO](http://en.wikipedia.org/wiki/ISO_3166-1)

+ Request 

    + Headers
        
            x-bwin-accessId: [AccessId]
            
+ Response 200

+ Response 400

        {
            "code": 102,
            "message": "Invalid country"
        }

+ Response 403

        {
            "code": 456,
            "message": "Blocked by country rule",
            "values": 
            [
                {"Key":"TargetUrl", "Value": "https://www.bwin.fr"},
                {"Key":"SitecorePath", "Value": ""}
            ]
        }


## Login [/Authentication.svc/Login]

A customer login is performed by sending a `POST` request to the PosAPI authentication endpoint.
The body is a simple “Login” element with username and password, as well as `language` and `deviceID` as optional attributes.
In case the `language` parameter is missing, the default language will be used.
 
The `deviceID` parameter is used to pass along the ID of mobile devices, and by doing so associating the created session with the device (used for DWH).

The `dateOfBirth` parameter is only required for labels with that regulatory requirement (e.g. bwin.fr).

By passing along a `handshakeSessionKey`, the authentication request gets associated with an already available session in the backend.
If this parameter is part of the request sent to the backend, the interceptor-stack of the backend will not be executed, and the password-parameter can be empty.
This is used for example for the 3rd-party integration with *DanskeSpil*.

> **Note:** There are various so-called interceptors which may deny a successful authentication. Those are handled in a [Workflow](#authentication-workflow-ids), the user has to complete before being fully authenticated.
> The `workflowType` parameter of the response exposes which workflow the user is in (`0` if none).
    
### Login a User [POST]

- `username`: (string) - The username of the account.
- `password`: (string) - The password associated with the account.
- `language`: (optional, string) - The twoletter lanague code.
- `deviceId`: (optional, string) - Can be used by mobile clients.
- `dateOfBirth`: (optional, dateTime) - Mandatory for french labels.
- `handshakeSessionkey`: (optional, string)
- `ucid`: (optional, string)

+ Request (application/json)

    + Headers
        
            x-bwin-accessId: [AccessId]
    
    + Body
            
            {
                "username": "myUserName",
                "password": "password",
                "language": "EN",
                "deviceId": "ABC123",
                "dateOfBirth": "/Date(1315996382000)/",
                "handshakeSessionKey": "7401fefbad9a4f87a40e5d54930c285d",
                "ucid": "12345abcd",
                "fingerprint": {
                    "superCookie":"superSecretSuperCookie",
                    "deviceDetails":[
                        {"Key":"dt", "Value":"website"},
                        {"Key":"os", "Value":"Windows"}
                    ]
                }
            }
    
+ Request (application/xml)

    + Headers
        
            x-bwin-accessId: [AccessId]

    + Body

            <Login 
              username="username" 
              password="password" 
              language="EN" 
              deviceId="ABC123" 
              dateOfBirth="1997-06-13" 
              handshakeSessionKey="7401fefbad9a4f87a40e5d54930c285d"
              ucid="12345abcd"
            />


+ Response 200 (application/json)

        {  
           "Balance":{  
              "accountBalance":1250.01,
              "accountCurrency":"EUR",
              "balanceForGameType":0,
              "bonusWinningsRestrictedBalance":0,
              "cashoutRestrictedBalance":0,
              "cashoutableBalance":1250.01,
              "depositRestrictedBalance":0,
              "inPlayAmount":0,
              "owedAmount":0,
              "playMoneyBalance":0,
              "playMoneyInPlayAmount":0,
              "releaseRestrictedBalance":0
           },
           "ClaimValues":[  
              {  
                 "Key":"SampleClaimKey",
                 "Value":"SampleClaimValue"
              }
           ],
           "PendingActions":{  
              "Actions":null,
              "ordered":false
           },
           "PostLoginValues":null,
           "SogeiCustomerId":null,
           "accountCategoryId":0,
           "accountId":110750845,
           "globalSessionId":null,
           "language":"EN",
           "lastLoginUTC":"/Date(0)/",
           "lastLogoutUTC":"/Date(0)/",
           "playerCategory":1,
           "realPlayer":true,
           "rsaAssigned":false,
           "screenName":"sntokenguy1",
           "serverTimeUTC":"/Date(1415267177020)/",
           "serviceSessionId":null,
           "sessionToken":[SessionToken],
           "ssoToken":[SsoToken],
           "userName":"tokenguy1",
           "userToken":[UserToken],
           "workflowType":0,
           "workflowKeys":null,
           "superCookie":"topSecret"
        }


+ Response 403
    
        {
            "code": 600,
            "message": "Authentication failed"
        }

## PID-Login [/Authentication.svc/Login/PID]

PID based login for DanID based labels. 
    
### Login a User [POST]

- `pid`: (string) - The username of the account.
- `username`: (optional, string) - The username of the account (required only when called directly after registration).
- `username`: (optional, string) - The password of the account (required only when called directly after registration).
- `language`: (optional, string) - The twoletter lanague code.
- `deviceId`: (optional, string) - Can be used by mobile clients.
- `ucid`: (optional, string)

+ Request (application/json)

    + Headers
        
            x-bwin-accessId: [AccessId]
    
    + Body
            
            {
                "pid": "123asdf",
                "language": "EN",
                "username": "denMark",
                "password": "passme",
                "deviceId": "ABC123",
                "ucid": "12345abcd",
                "fingerprint":{ /*see login*/ }
            }
    

+ Response 200 (application/json)

        {  
           "Balance":{  
              "accountBalance":1250.01,
              "accountCurrency":"EUR",
              "balanceForGameType":0,
              "bonusWinningsRestrictedBalance":0,
              "cashoutRestrictedBalance":0,
              "cashoutableBalance":1250.01,
              "depositRestrictedBalance":0,
              "inPlayAmount":0,
              "owedAmount":0,
              "playMoneyBalance":0,
              "playMoneyInPlayAmount":0,
              "releaseRestrictedBalance":0
           },
           "ClaimValues":[  
              {  
                 "Key":"SampleClaimKey",
                 "Value":"SampleClaimValue"
              }
           ],
           "PendingActions":{  
              "Actions":null,
              "ordered":false
           },
           "PostLoginValues":null,
           "SogeiCustomerId":null,
           "accountCategoryId":0,
           "accountId":110750845,
           "globalSessionId":null,
           "language":"EN",
           "lastLoginUTC":"/Date(0)/",
           "lastLogoutUTC":"/Date(0)/",
           "playerCategory":1,
           "realPlayer":true,
           "rsaAssigned":false,
           "screenName":"sntokenguy1",
           "serverTimeUTC":"/Date(1415267177020)/",
           "serviceSessionId":null,
           "sessionToken":[SessionToken],
           "ssoToken":[SsoToken],
           "userName":"tokenguy1",
           "userToken":[UserToken],
           "workflowType":0,
           "workflowKeys":null
        }


+ Response 403
    
        {
            "code": 600,
            "message": "Authentication failed"
        }



## Logout [/Authentication.svc/Logout]

### Logout a User [POST]

+ Request (application/json)

    + Headers 
        
            x-bwin-accessId: [AccessId]
            x-bwin-session-token: [SessionToken]
            x-bwin-user-token: [UserToken]
                        
+ Response 200 (application/json)

+ Response 400 (application/json)

        {
            "code": 201,
            "message": "Invalid user security token"
        }

## Cancel Workflow [/Authentication.svc/CancelWorkflow]

### Cancel a workflow [POST]

+ Request (application/json)

    + Headers
        
            x-bwin-accessId: [AccessId]
            x-bwin-session-token: [SessionToken]
            x-bwin-user-token: [UserToken]

+ Response 200 (application/json)

+ Response 404 (application/json)

        {
            "code": 630,
            "message": "No workflow pending"
        }

## Authorization [/Authorization]

If there is a pending workflow for a user, this user is authenticated but not authorized. It is not possible to call all PosAPI endpoints in this user state, but only all workflow relevant endpoints – e.g. finalizeWorkflow, get and set the screen name …

#### Authentication
| Status Code | Error Code | Message                       | Values/Description |
|:-----------:|:----------:|-------------------------------|--------------------|
| `403`       | `600`      | Authentication failed         |                    |
| `403`       | `611`      | User Just Blocked             |                    |
| `403`       | `612`      | Password expired              |                    |
| `403`       | `613`      | Account hacking suspect       |                    |
| `403`       | `614`      | User too young                | Only for BE users  |
| `403`       | `615`      | Token sent with password      |                    |
| `403`       | `690`      | Missing Language parameter    |                    |
| `403`       | `692`      | Invalid handshake session key |                    |
| `403`       | `698`      | User currency unknown         |                    |
| `403`       | `629`      | Device is blocked             |                    |

#### Responsible Gaming
| Status Code | Error Code | Message                       | Values/Description |
|:-----------:|:----------:|-------------------------------|--------------------|
| `403`       | `601`      | RG Closed                     |                                                                        |
| `403`       | `602`      | RG Cool off                   | <pre>{<br/>  "Key":"ReopenDate",<br/>  "Value":"2020-01-01T00:00:00"<br/>}</pre> |
| `403`       | `603`      | Player is temporarily blocked |                                                                        |

#### Login
| Status Code | Error Code | Message                 | Values/Description                                             |
|:-----------:|:----------:|-------------------------|----------------------------------------------------------------|
| `403`       | `605`      | Password blocked        |                                                                |
| `403`       | `606`      | Token blocked             |                                                                |
| `403`       | `607`      | SqSa blocked             |                                                                |
| `403`       | `608`      | Sll blocked             | <pre>{<br/>  "Key":"SllContactNo",<br/>  "Value":"122345-23"<br/> }</pre> |
| `403`       | `609`      | Manually blocked        |                                                                |
| `403`       | `610`      | Unknown technical error |                                                                |

#### KYC
| Status Code| Error Code | Message                                   |
|:----------:|:----------:|-------------------------------------------|
| `403`      | `621`      | Not KYC verified and grace period expired |
| `403`      | `622`      | KYC blocked category                      |
| `403`      | `623`      | KYC suspended category                    |
| `403`      | `624`      | KYC closed category                       |
| `403`      | `625`      | KYC doc not verified                      |
| `403`      | `626`      | KYC id not verified                       |
| `403`      | `627`      | KYC technical error                       |
| `403`      | `628`      | Maximum KYC attempts exceeded             |

#### Unfinished Registration
| Status Code | Error Code | Message                 | Values/Description                                                                                      |
|:-----------:|:----------:|-------------------------|---------------------------------------------------------------------------------------------------------|
| `403`       | `635`      | Unfinished registration | Depending on Brand configuration – [WorkflowID 7](#authentication-workflow-ids) or this error is fired. |

#### Migration
| Status Code | Error Code | Message                                |
|:-----------:|:----------:|----------------------------------------|
| `403`       | `650`      | User migration update profile failed   |
| `403`       | `651`      | User migration update profile suspened |
| `403`       | `652`      | User migration technical error         |
| `403`       | `653`      | User migration duplicate account       |

#### DateOfBirth
| Status Code | Error Code | Message                                |
|:-----------:|:----------:|----------------------------------------|
| `403`       | `655`      | Date of birth blocked                  |

#### Max Login
| Status Code | Error Code | Message                                |
|:-----------:|:----------:|----------------------------------------|
| `403`       | `656`      | Maximum of logged in users reached     |

#### Inactive Player
| Status Code | Error Code | Message                                          |
|:-----------:|:----------:|--------------------------------------------------|
| `403`       | `657`      | Player has been inactive for more than 6 months. |

#### User Attributes
| Status Code | Error Code | Message                                |
|:-----------:|:----------:|----------------------------------------|
| `403`       | `660`      | Fraud                                  |
| `403`       | `661`      | Colluder                               |
| `403`       | `662`      | Closed                                 |
| `403`       | `663`      | Suspicious                             |
| `403`       | `664`      | Closed on request                      |
| `403`       | `665`      | Reopened on request                    |
| `403`       | `666`      | RG closed                              |
| `403`       | `667`      | Arjel blocked                          |
| `403`       | `668`      | Permanent Closed                       |
| `403`       | `669`      | Sportsbook Fraud                       |

#### Suspicious Location
| Status Code | Error Code | Message                                |
|:-----------:|:----------:|----------------------------------------|
| `403`       | `670`      | SLL max failed attempts reached        |
| `403`       | `671`      | SLL validation failed                  |
| `403`       | `672`      | SLL contact update success             |

#### IP/UCID Block
| Status Code | Error Code | Message         | Values/Description                                                 |
|:-----------:|:----------:|-----------------|--------------------------------------------------------------------|
| `403`       | `675`      | IP blocked      |                                                                    |
| `403`       | `676`      | UCID blocked    |                                                                    |
| `403`       | `677`      | Subnet blocked  |                                                                    |
| `403`       | `678`      | Country blocked | For SH:<br/><pre>{<br/>  "Key":"SubErrorCode",<br/>  "Value":"64073"<br/> }</pre> |

#### Security Token
| Status Code | Error Code | Message                                                  |
|:-----------:|:----------:|----------------------------------------------------------|
| `403`       | `680`      | Invalid security token password entered                  |
| `403`       | `681`      | Display security token player block message              |
| `403`       | `682`      | Security token not required for login                    |
| `403`       | `683`      | Display OTP screen                                       |
| `403`       | `684`      | Security Token screen expired                            |
| `403`       | `685`      | Invalid security token code entered                      |
| `403`       | `686`      | Unknown error when validating security token information |
| `403`       | `687`      | Security Token information required                      |

#### Workflow
| Status Code | Error Code | Message                                             |
|:-----------:|:----------:|-----------------------------------------------------|
| `403`       | `630`      | No Workflow pending                                 |
| `403`       | `631`      | Backend Error - User logged out                     |

#### AutoLogin
| Status Code | Error Code | Message                                             |
|:-----------:|:----------:|-----------------------------------------------------|
| `403`       | `640`      | Service session creation failed                     |
| `403`       | `641`      | Interceptors pending launch url                     |
| `403`       | `642`      | Login service unavailable                           |
| `403`       | `643`      | Cross domain                                        |
| `403`       | `644`      | General Error                                       |

#### SQSA Login Interceptor
| Status Code | Error Code | Message                                             |
|:-----------:|:----------:|-----------------------------------------------------|
| `403`       | `1601`     | SqSa Failure Invalid data                           |
| `403`       | `1602`     | SqSa Failure Threshold reached                      |
| `403`       | `1603`     | Unknown technical error                             |

#### User Acknowledgement Interceptor
| Status Code | Error Code | Message                                             |
|:-----------:|:----------:|-----------------------------------------------------|
| `403`       | `1610`     | User acknowledgement Unknown error                  |

#### KYC BE Interceptor
| Status Code | Error Code | Message                                             |
|:-----------:|:----------:|-----------------------------------------------------|
| `400`       | `1800`     | Invalid passport entered                            |
| `400`       | `1801`     | Invalid profession entered                          |
| `400`       | `1802`     | National registration number and passport are empty |
| `400`       | `1803`     | Invalid passport length                             |
| `400`       | `1804`     | Invalid place of birth                              |
| `400`       | `1805`     | Invalid national registration number length         |
| `400`       | `1806`     | Invalid place of birth length                       |
| `400`       | `1807`     | Invalid national registration number                |
| `403`       | `1808`     | User is blacklisted                                 |
| `400`       | `1809`     | User already registered with regulatory             |
| `403`       | `1810`     | Unknown technical error                             |

#### StrongAuth Interceptor
| Status Code | Error Code | Message                       | Values/Description                                             |
|:-----------:|:----------:|-------------------------------|----------------------------------------------------------------|
| `403`       | `1902`     | Otp regeneration failed       | <pre>[{<br/>"Key":"USER_OTP_CURRENT_REGENERATION_COUNT",<br/>"Value":"1"<br/>},<br/>{<br/>"Key":"OTP_MAX_REGENERATE_COUNT",<br/>"Value":"2"<br/>}]</pre>                                                     |
| `403`       | `1903`     | Invalid data                  |                                                                |
| `403`       | `1905`     | Otp cooled off                   | <pre>[{<br/>"Key":"OTP_MAX_ATTEMPT_COUNT",<br/>"Value":"5"<br/>},<br/> {<br/>"Key":"OTP_COOL_OFF_TIME",<br/>"Value":"30"<br/> },<br/> {<br/>  "Key":"OTP_MAX_REGENERATE_COUNT",<br/>"Value":"2"<br/> }]</pre> |
| `403`       | `1906`     | Otp max regeneration reached  |                                                                |
| `403`       | `1907`     | Otp expired                   | <pre>[{<br/>"Key":"OTP_MAX_ATTEMPT_COUNT",<br/>"Value":"5"<br/>},<br/> {<br/>"Key":"OTP_CURRENT_FAILED_ATTEMPTS",<br/>"Value":"1"<br/> },<br/> {<br/>  "Key":"OTP_MAX_REGENERATE_COUNT",<br/>"Value":"2"<br/> }]</pre> |
| `403`       | `1909`     | Unknown technical error       |                                                                |
| `500`       | `1908`     | Backend technical error       |                                                                |

#### Funds Regulation Interceptor
| Status Code | Error Code | Message                           | Values/Description                                                                                          |
|:-----------:|:----------:|-----------------------------------|------------------------------------------------------------------------------------------------------------ |
| `403`       | `1630`     | Terms and Condition update failed |                                                                                                             |
| `403`       | `1631`     | Fund Protection update failed     |                                                                                                             |
| `403`       | `1632`     | Daily Limit update failed         |                                                                                                             |
| `403`       | `1633`     | Weekly Limit update failed        |                                                                                                             |
| `403`       | `1634`     | Monthly Limit update failed       |                                                                                                             |
| `403`       | `1635`     | Invalid Daily Limit               |                                                                                                             |
| `403`       | `1636`     | Invalid Weekly Limit              |                                                                                                             |
| `403`       | `1637`     | Invalid Monthly Limit             |                                                                                                             |
| `403`       | `1638`     | Overall update failed             |                                                                                                             |
| `403`       | `1639`     | Maximum limit exceeded            | <pre>[{<br/>"Key":"MAXIMUMLIMIT",<br/>"Value":"1000"<br/>},<br/>{<br/>"Key":"CURRENCY",<br/>"Value":"EUR"<br/>}]</pre> |

#### KYC SH Interceptor
| Status Code | Error Code | Message                           |
|:-----------:|:----------:|-----------------------------------|
| `403`       | `1640`     | Unknown technical error           |
| `400`       | `1641`     | Invalid length for city of birth  |
| `400`       | `1642`     | City of birth is null or empty    |
| `400`       | `1643`     | Nationality is null or empty      |

#### Denmark Blacklist Check Interceptor
| Status Code | Error Code | Message                           |
|:-----------:|:----------:|-----------------------------------|
| `403`       | `1645`     | Unknown technical error           |
| `400`       | `1646`     | User blocked by Danish blacklist  |

#### UpdateSQSA Interceptor
| Status Code | Error Code | Message                           |
|:-----------:|:----------:|-----------------------------------|
| `403`       | `1620`     | UpdateSqSa Failure Invalid data   |
| `403`       | `1621`     | UpdateSqSa update failed          |
| `403`       | `1622`     | Unknown technical error           |

#### DkPasswordCheck
| Status Code | Error Code | Message                           |
|:-----------:|:----------:|-----------------------------------|
| `403`       | `500`      | Password does not match criteria  |
| `500`       | `101`      | Unknown error code: [number]      |

#### PidVerification
| Status Code | Error Code | Message                                 |
|:-----------:|:----------:|-----------------------------------------|
| `403`       | `1662`     | User credentials and PID are required   |

#### LoginCountLimit
| Status Code | Error Code | Message                                           |
|:-----------:|:----------:|---------------------------------------------------|
| `403`       | `1666`     | Login count limit updated and login restricted    |

#### UpdateEmail Interceptor
| Status Code | Error Code | Message                              |
|:-----------:|:----------:|--------------------------------------|
| `500`       | `4301`     | Technical error while updating email |
| `400`       | `4302`     | Email already in use                 |
| `400`       | `4303`     | Old email submitted again            |
| `400`       | `4304`     | Email address invalid                |

#### Intra Migration
| Status Code | Error Code | Message                               |
|:-----------:|:----------:|---------------------------------------|
| `403`       | `1690`     | User has been migrated to Premiumbull |

#### Secure Login
Sub error codes:
| SubError | Reason                                   |
|:--------:|------------------------------------------|
| `11`     | OTP Entered by player has expired.       |
| `12`     | Invalid OTP ID /Request ID.              |
| `13`     | Invalid OTP code or ID                   |
| `14`     | Maximum OTP validation count reached     |
| `15`     | OTP validation failed                    |
| `16`     | Mobile number entered by user is invalid.|
| `18`     | Regeneration of new OTP  failed          |
| `19`     | Generation of  new OTP  failed           |
| `20`     | OTP code generation is success           |
| `21`     | User is in cool off state                |
| `22`     | Maximum OTP regeneration count reached   |
| `23`     | ID /Request ID has expired.              |
| `24`     | Communication failure from OTP Service.  |
| `111`    | Mobile number is empty.                  |
| `112`    | Mobile number update failed.             |

Error codes:
| ErrorCode | Reason                                 |
|:---------:|----------------------------------------|
| `119`     | Regular OTP flow                       |
| `122`     | Ask user to opt in /out / remind later |

Workflow keys:
| Workflow key     | Description           | When to send                      |
|:----------------:|-----------------------|-----------------------------------|
| REQUEST_ID       | The OTP request id    | With every request                |
| otpCode          | The received otp code | on OTP verify                     |
| sendThroughEmail | boolean if resend should be by email | to resend OTP      |
| OPT_FLAG         | If the user opts in or out of 2FA. Values are "OPT_IN", "OPT_OUT" or "REMIND_ME_LATER" | errorCode 122 |
| countryCode      | mobile country code of the user | If OPT_FLAG is "OPT_IN" |
| Mobilenumber     | mobile number of the user       | If OPT_FLAG is "OPT_IN" |

#### General
| Status Code | Error Code | Message                 |
|:-----------:|:----------:|-------------------------|
| `400`       | `695`      | User Currency invalid   |
| `400`       | `696`      | User Country invalid    |


## Workflow IDs [/Workflows]
| ID   | Workflow Name                      | API endpoints to resolve the workflow                                                                                                                                                                 |
|:----:|------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `0`  | No Workflow pending                |                                                                                                                                                                                                       |
| `1`  | TermsAndConditionsUpdated          | [Sh/Details](#kyc-schleswig-holstein-details), [FinalizeWorkflow](#authentication-finalize-workflow) (To set maidenname in *BwinSh*)<br/>[FinalizeWorkflow](#authentication-finalize-workflow)        |
| `2`  | Kyc                                | [KycStatus](#kyc-kyc-status), [KycData](#kyc-kyc-data), [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                         |
| `3`  | ScreenName                         | [ScreenName](#account-screenname), [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                              |
| `4`  | SLL (Suspicious Location Login)    | [SllQuestions](#account-ssl-questions), [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                         |
| `5`  | UserMigration                      | [MigrateUser](#account-migrate-user), [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                           |
| `6`  | SecurityToken                      | [WorkflowData](#authentication-add-workflow-data) with keys `countryCode` and `mobilenumber`, then [FinalizeWorkflow](#authentication-finalize-workflow) to accept migration to OTP; [SkipWorkflow](#authentication-skipworkflow) to stay with RSA key|
| `7`  | FinalizeRegistration               | [UserData](#account-userdata), [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                                  |
| `8`  | BirthLocation                      | [BirthLocation](#kyc-birth-location), [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                           |
| `9`  | SpainMigration                     | [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                                                                 |
| `10` | SqSaLogin                          | [SecurityAnswers](#authentication-securityanswers), [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                             |
| `11` | UserAcknowledgement                | [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                                                                 |
| `12` | KycBe                              | [Be/Details](#kyc-belgium-details), [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                             |
| `13` | StrongAuth                         | [StrongAuthentication](#authentication-strongauthentication), [FinalizeWorkflow](#authentication-finalize-workflow)<br/>[FinalizeWorkflow](#authentication-finalize-workflow) (Password regeneration) |
| `14` | PremiumMigrationOffer              | [FinalizeWorkflow](#authentication-finalize-workflow) (user wants to migrate)<br/>[SkipWorkflow](#authentication-skipworkflow) (user doesn’t want to migrate)<br/>See [PostLostinValues](#authentication-postlogin-values) |
| `15` | PremiumMigrationCompletion         | [FinalizeWorkflow](#authentication-finalize-workflow) (if the user gets redirected to *Premium*, Logout should be called afterwards by frontend)<br/>See [PostLostinValues](#authentication-postlogin-values)              |
| `16` | UpdateSqSa                         | [SecurityAnswers](#authentication-securityanswers), [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                             |
| `17` | PremiumMigrationInformation        | [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                                                                 |
| `18` | KycUk                              | [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                                                                 |
| `19` | FundsRegulation                    | [Uk/RegulationData](#account-uk-regulation-data), [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                               |
| `20` | ItRegulationKO                     | [It/UserData](#account-italian-user-data), [FinalizeWorkflow](#authentication-finalize-workflow)<br/>[SkipWorkflow](#authentication-skipworkflow) (remind later)                                      |
| `21` | KycSh                              | [Sh/Details](#kyc-schleswig-holstein-details), [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                  |
| `22` | IncorrectNrn                       | [NationalRegistrationDetails](#kyc-national-registration-details), [FinalizeWorkflow](#authentication-finalize-workflow)<br/>See [PostLostinValues](#authentication-postlogin-values)                 |
| `23` | PostMigrationTermsAndConditions    | [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                                                                 |
| `24` | UserAcknowledgement2               | [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                                                                 |
| `25` | DepositLimitConfirmation           | [DepositLimitConfirmation](#account-depositlimitconfirmation), [FinalizeWorkflow](#authentication-finalize-workflow)<br/>[SkipWorkflow](#authentication-skipworkflow) (remind later)<br/>See [PostLostinValues](#authentication-postlogin-values)|
| `26` | RealityCheckPreferences            | [RealityCheckPreferences](#account-realitycheckpreferences), [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                    |
| `27` | TncCnpRo                           | [NationalRegistrationDetails](#kyc-national-registration-details), [FinalizeWorkflow](#authentication-finalize-workflow)                                                                              |
| `28` | DkPasswordCheck                    | [Password](#authentication-password), [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                           |
| `29` | TncAndIntendedGamingActivity       | [Dk/Details](#kyc-denmark-details), [FinalizeWorkflow](#authentication-finalize-workflow)<br/>See [PostLostinValues](#authentication-postlogin-values) for *TNC_AND_IGA_REASON*                       |
| `30` | PidVerification                    | [Dk/MapAccountToPid](#kyc-map-account-to-pid), [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                  |
| `31` | DepositLimitIncrementConfirmation  | [DepositLimitIncrementConfirmation](#account-depositlimitincrementconfirmation), [FinalizeWorkflow](#authentication-finalize-workflow)<br/>[SkipWorkflow](#authentication-skipworkflow) (remind later)<br/>See [PostLostinValues](#authentication-postlogin-values)|
| `32` | DepositLimitVerification           | [FinalizeWorkflow](#authentication-finalize-workflow) (user wants to proceed with document upload)<br/>[SkipWorkflow](#authentication-skipworkflow) (remind later)<br/>See [PostLoginValues](#authentication-postlogin-values) (*DEPOSIT_GRACE_DAYS*)|
| `33` | LoginCountLimit                    | [Limits/Player](#responsible-gaming-player-limits-post) and [Logout](#authentication-logout)                                                                                                          |
| `34` | PlayerLimits                       | [Limits/Player](#responsible-gaming-player-limits-post) and [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                     |
| `35` | AdditionalInfo                     | [WorkflowData](#authentication-add-workflow-data) (keys `birthNumber`, `nationality`, `idCardNumber`, `idCardValidFrom`, `idCardValidUntil`), [FinalizeWorkflow](#authentication-finalize-workflow)   |
| `36` | LoginDurationExceeded              | [FinalizeWorkflow](#authentication-finalize-workflow) or [Limits/Player](#responsible-gaming-player-limits-delete) (user wants to opt out (7days waiting period))<br/> and [Logout](#authentication-logout) |
| `37` | LoginCooloff                       | [FinalizeWorkflow](#authentication-finalize-workflow) or [Limits/Player](#responsible-gaming-player-limits-delete) (user wants to opt out (7days waiting period))<br/> and [Logout](#authentication-logout) <br/>See [PostLoginValues](#authentication-postlogin-values) (*WAIT_TIME_FOR_COOLOFF_END*)|
| `38` | TwoFactorAuthentication            | Ask user to setup otp flow. [Update 2FA Preferences](#authentication-two-factor-authentication-post), then [FinalizeWorkflow](#authentication-finalize-workflow)                                      |
| `39` | TwoFactorOtp                       | Otp was sent to user, query for received Otp. [Verify 2FA Otp](#authentication-verify-2fa-otp) or [Resend 2FA Otp](#authentication-resend-2fa-otp), then [FinalizeWorkflow](#authentication-finalize-workflow) |
| `40` | RG Limits Test                     | [WorkflowData](#authentication-add-workflow-data) with key `testResult` as `PASS`, `FAIL` or `LATER`; then [FinalizeWorkflow](#authentication-finalize-workflow)                                      |
| `41` | KYC Germany                        | [FinalizeWorkflow](#authentication-finalize-workflow) to accept, [SkipWorkflow](#authentication-skipworkflow) to remind later                                                                         |
| `42` | Nationality                        | [WorkflowData](#authentication-add-workflow-data) with key `nationality` (two-letter country code); then [FinalizeWorkflow](#authentication-finalize-workflow)                                        |
| `43` | UpdateEmail                        | [WorkflowData](#authentication-add-workflow-data) with key `newEmail`; then [FinalizeWorkflow](#authentication-finalize-workflow)                                                                     |
| `44` | Secure Login                       | [WorkflowData](#authentication-add-workflow-data), then [FinalizeWorkflow](#authentication-finalize-workflow). Watch the `subErrorCode` in response data (see above)                                  |
| `45` | Captcha Confirm                    | [FinalizeWorkflow](#authentication-finalize-workflow). Possible responses are Login forbidden (same as invalid username/pwd) or Login success                                                         |
| `46` | Message Preferences Update         | [WorkflowData](#authentication-add-workflow-data) with keys the same as the communication settings received as part of login response, then [FinalizeWorkflow](#authentication-finalize-workflow)     |
| `47` | France Kyc Reminder                | [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                                                                 |
| `48` | Gamestop UK RG Check               | [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                                                                 |
| `49` | Italy KYC Reminder                 | [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                                                                 |
| `50` | Inta-migrated                      | Not clear-able                                                                                                                                                                                        |
| `51` | FrLicenseChange                    | [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                                                                 |
| `52` | ShKycCheck							| [FinalizeWorkflow](#authentication-finalize-workflow)                                                                                                                                                 |
| `53` | BeKycCheck							| [WorkflowData](#authentication-add-workflow-data) with updated user data, then [FinalizeWorkflow](#authentication-finalize-workflow); Or [SkipWorkflow](#authentication-skipworkflow) to skip (if allowed) |


## PostLogin Values [/PostLoginValue]
| Key                                   | Value                           | Description                                                                                                                                                                     |
|---------------------------------------|---------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `MIGRATION_KYC_SUB_ERROR_CODE`        | `109`                           | KYC verification is not done. Third party service unavailable.                                                                                                                  |
|                                       | `110`                           | User Identity is incorrect, and KYC verification is failed.                                                                                                                     |
|                                       | `121`                           | KYC verification is all success.                                                                                                                                                |
| `MIGRATION_WALLET_TRANSFER`           | `1`                             | Migration is success.                                                                                                                                                           |
|                                       | `2`                             | Failed                                                                                                                                                                          |
|                                       | `3`                             | Migration skipped -will be due to KYC verification failure.<br/>MIGRATION _KYC_SUB_ERROR_CODE will be available to know the reason, if required.                                |
| `MIGRATION_USER_PROFILE`              | `1`                             | Profile migration success.                                                                                                                                                      |
|                                       | `2`                             | Profile migration failed.                                                                                                                                                       |
| `MAX_KYC_ATTEMPTS_REACHED`            | `True` or `False`               | Player reached the maximum attempts to get KYC verified.                                                                                                                        |
| `PLAYER_KYC_ATTEMPTS_LEFT`            | `Number`                        | How many times is user allowed to proceed without providing the required details                                                                                                |
| `STRONG_AUTH_TYPE`                    | `ShowStrongAuth`                | Password generated and mail sent successfully. Screen to enter password need to be shown to user.                                                                               |
|                                       | `StrongAuthValidationFailed`    | Password validation failed.                                                                                                                                                     |
|                                       | `StrongAuthRegenerationSuccess` | Password regenerated successfully and sent to user.                                                                                                                             |
| `OTP_MAX_ATTEMPT_COUNT`               | `Number`                        | Maximum threshold value for the password failure. After exceeding this threshold value the user will be in cool off state.                                                      |
| `OTP_MAX_REGENERATE_COUNT`            | `Number`                        | Maximum threshold value for password regeneration. Regeneration will be failed by Login Service if retry attempt exceeds this value in the session.                             |
| `OTP_CURRENT_FAILED_ATTEMPTS`         | `Number`                        | Current failure attempt count                                                                                                                                                   |
| `USER_OTP_CURRENT_REGENERATION_COUNT` | `Number`                        | Current password regeneration count. Session specific.                                                                                                                          |
| `MIGRATION_STATUS`                    | `OFFERED`, `COMPLETED`, `FAILED`| The current status of the Premium Migration. In step one this will be OFFERED, and depending of success/failure of the migration COMPLETED/FAILED will be returned in step two. |
| `MIGRATION_TYPE`                      | `SOFT`, `SEMI_HARD`, `HARD`     | The type of migration (Premium Migration)                                                                                                                                       |
| `MIGRATION_TARGET`                    | e.g. `pa`                       | The frontend id of the target label (Premium Migration)                                                                                                                         |
| `MIGRATION_PRODUCT`                   | e.g. `CASINO`                   | The product id on the target label after migration (Premium Migration)                                                                                                          |
| `MESSAGE_TYPE`                        | `messageTypeA`, `messageTypeB`  | The type of message to be shown to the user (Premium Migration)                                                                                                                 |
| `REWARD_TYPE`                         | `1`                             | The reward is a *cash gift*. (Premium Migration)                                                                                                                                |
|                                       | `2`                             | The reward is a *deposit bonus*. (Premium Migration)                                                                                                                            |
| `REWARD_CURRENCY`                     | e.g. `EUR`, `USD`               | The currency of the reward. (Premium Migration)                                                                                                                                 |
| `REWARD_MAXVALUE`                     | e.g. `10.50`                    | The max bonus amount for *deposit bonus*, otherwise the absolute amount in case of *cash gift* (string representation of decimal value). (Premium Migration)                    |
| `REWARD_PERCENTAGE`                   | `0`                             | No reward percentage specified or available. (Premium Migration)                                                                                                                |
|                                       | `1-100`                         | The reward percentage in case of *deposit bonus with percentage*. (Premium Migration)                                                                                           |
| `NO_OF_HOURS_LEFT`                    | `1-72`                          | The hours the user has left to validate his account by uploading a document (for KYC UK workflow)                                                                               |
| `BELGIUM_NRN_REASON`                  | `0`                             | Default                                                                                                                                                                         |
|                                       | `1`                             | Invalid national registration number                                                                                                                                            |
|                                       | `2`                             | Invalid passport number                                                                                                                                                         |
|                                       | `3`                             | An account with these details already exists                                                                                                                                    |
|                                       | `4`                             | An account doesn't have both documents                                                                                                                                          |
|                                       | `9`                             | Unable to validate national registration or passport number due to technical issues                                                                                             |
| `LIMIT_CURRENCY`                      | `EUR`                           | Currency of the deposit limit value                                                                                                                                             |
| `LIMIT_DAILY_CURRENT`                 | `80.00`                         | Current daily deposit limit                                                                                                                                                     |
| `LIMIT_DAILY_PENDING`                 | `85.00`                         | Pending daily deposit limit                                                                                                                                                     |
| `LIMIT_WEEKLY_CURRENT`                | `500.00`                        | Current weekly deposit limit                                                                                                                                                    |
| `LIMIT_WEEKLY_PENDING`                | `600.00`                        | Pending weekly deposit limit                                                                                                                                                    |
| `LIMIT_MONTHLY_CURRENT`               | `2000.00`                       | Current monthly deposit limit                                                                                                                                                   |
| `LIMIT_MONTHLY_PENDING`               | `2500.00`                       | Pending monthly deposit limit                                                                                                                                                   |
| `RG_DEPOSIT_LIMIT_LEVEL_CHANGE`       | `true` or `false`               | Whether the limit change resulted in a level change for the user (differnt UI to be displayed)                                                                                  |
| `TNC_AND_IGA_REASON`                  | `0`                             | Tnc Acceptance and Intended Gaming Activity missing                                                                                                                             |
|                                       | `1`                             | Tnc Acceptance missing                                                                                                                                                          |
|                                       | `2`                             | Intended Gaming Activity missing                                                                                                                                                |
| `DEPOSIT_GRACE_DAYS`                  | `42`                            | Number of days left to get fully verified                                                                                                                                       |
| `LEFT_OVER_DAILY_LOGIN_DURATION`      | `1234`                          | Remaining session time until logout (in seconds)                                                                                                                                |
| `WAIT_TIME_FOR_COOLOFF_END`           | `234`                           | Time until the user can login again (in seconds)                                                                                                                                |
| `LIMIT_ACTIVATION_WAIT_PERIOD`        | `56`                            | The waiting period (units unknown)                                                                                                                                              |