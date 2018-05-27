HOST: api.pos.com/V3
FORMAT: 1A

# PosAPI Documentation V3
The **bwin.party Point of Sale API (PosAPI)** is a set of APIs that allow for the development of various clients 
while providing security and customizability for the clients’ needs and striving to offering sane interfaces for 
(nearly) all functionality necessary to implement such a client.


This document deals with the common concepts of using these APIs and the specific functionalities of the 
so called **Platform PosAPI** (also known as **PPos**) which is used for authentication, access to user settings 
and common functionalities like retrieving lists of countries and currencies that are valid for a given client.

<!-- include(/BoxOffice.Api/authentication.md) -->

# Group Appendix

## REST Principles [/RESTPrinciples]

The APIs are based on REST principles. REST (Representational State
Transfer) is the architectural style that is underlying the World Wide
Web. The REST architectural style emphasizes some principles that are
also applicable for Web services as are provided by the PosAPI.

Calls to the API are performed as HTTP requests and response is returned
via HTTP responses. Data is transmitted either in the request URL, or in
the request body. Additional data is transmitted in HTTP header fields.
 

+ **Resources:** The key abstraction within REST is the
resource. The API exposes any data element that can be identified via
some URLs as resources.

+ **Representations:** Different representations of a resource
may be requested by clients by providing media types and languages.

+ **Statelessness:** A Server does not maintain any client state
across HTTP requests. State is always provided via single requests.
Hence, the PosAPI service implementation does not expect that a request
is followed by subsequent requests. Though, global data may be persisted
across calls. This data is observable by any interested client.

 

Some REST principles were not satisfied:

+ **Idempotency:** GET requests to the same resource shall
always return the same data. However, data within the gaming domain is
rather stale due frequent update of betting odds. For simplifying
construction of clients the API does not guarantee that identical data
is returned.


## Request [/RequestFormat]

All requests are performed with one of the HTTP methods `GET`, `POST`, `PUT`,
or `DELETE` (`PUT` is currently not used but may be used in further releases
of the API).

#### Message 

PosAPI uses standard HTTP request messages as defined in [RFC 2616
Section 5](http://tools.ietf.org/html/rfc2616#section-5).

#### Format

A request format needs to be provided for every HTTP POST request. This
is done with the `Content-Type` HTTP header.

```http
Content-Type: application/xml
```

Accepted content-types are `application/xml` and `application/json`. 
If an empty or invalid content type is provided the HTTP Response Code
**415 Unsupported Media** Type is returned.


## Response [/ResponseFormat]

#### Message

PosAPI uses standard HTTP response messages as defined in [RFC 2616
Section 6](http://tools.ietf.org/html/rfc2616#section-6).

#### Format

A response that is returned by PosAPI is either formatted as *XML*, *JSON*
or as *JSONP* (JSON with padding). The return format is chosen by setting
the media type in the `Accept` Http-header field or the jsonp query
parameter. *JSONP* is only supported for `GET` requests. When the jsonp
query parameter is present it overrules the provided `Accept` header
field.

##### XML
```http
Content-Type: application/xml
```

##### JSON
```http
Content-Type: application/json
```

##### JSONP
```http
Content-Type: application/javascript
```

If an empty or invalid response format is requested the default type
“application/xml” will be used.


## Error Messages [/ErrorMessages]

Service operations return standard HTTP Status Codes as defined in [RFC
2616 Section 10](http://tools.ietf.org/html/rfc2616#section-10). More
error details are returned in the form of a response body in the
following format:

#### XML Error

```xml
<Error>
   <Code>IntegerValue</Code>
   <Message>StringMessage</Message>
</Error>
```

#### JSON Error

```json
{
    "code":IntegerValue,
    "message":"StringMessage"
}
```

## Language Selection [/LanguageSelection]

Every AccessId has a predefined language that is used by default - the so-called *defaultLanguageId*.
It is however possible to override this language, by setting a ***lang*** query-string parameter.

Clients can retrieve of supported languages by calling the PosAPI’s *Common* functionality endpoint for [languages](#common-language "languages").

The language is validated against the list of supported languages of the platform. E.g. when `lang=nl` is retrieved, PPOS validates if dutch is a supported language on that label.
If an invalid language is requested, http status code **400 Invalid Input** is returned with a corresponding errorCode and message.

### Format

Accepted formats for the language parameter is either the
- language id: unique two-letter code, e.g. `en`, `de`, `nl`
- dotNetCulture code: unique culture code, e.g. `en-US`, `en-GB`, `de-DE`

Invalid formats:
- platform language code: `en_US`, `en_GB`, `de_DE`
- sports language id: `1`, `2`, `17`


## API Versioning [/APIVersioning]

The Version of the API is encoded in the request URL after the endpoint. In this case it's **V3**.

```http
https://api.bwin.com/V3/Common.svc/Country 
```

## AccessIds [/AccessIds]

For identification purposes each client is assigned a unique AccessId.
Depending on this ID it is possible to set different behaviours and
access rights on API level. For instance, depending on requirements, it
is possible to block modules of different APIs and similar such
functionality.


Sending this AccessId with every request is mandatory. In case of
setting an invalid AccessId the following error code is returned:

#### Error
```http
HTTP/1.1 400 Bad Request
```
```json
{
    "code": 201,
    "message": "Invalid access security token [Token]."
}
```
 


The AccessId needs to be set to the `x-bwin-accessId` HTTP header field
or query-string parameter. Assuming that the AccessId sent to you was
XYZ9876ABC123, this would look like the samples below.

#### Example: Header

```http
x-bwin-accessId: XYZ987ABC123 
```

#### Example: Query-string Parameter

```http
https://api.bwin.com/V3/Common.svc/Country?x-bwin-accessId=XYZ987ABC123 
```


#### Getting an AccessId

> In order to **request** an AccessID please get in touch with your bwin.party contact.


## Remarks regarding samples [/Remarks]

Samples are trimmed down to only show custom-headers and other
standard values are assumed. For added clarity the `Host` and
`Content-Type` are left in place. Fell free to imagine the other headers
like `Content-Length`, `Cache-Control`, `Date`, and whatnot. 

> Throughout the documentation you will not find any valid AccessID or security tokens.

## Feedback [/Feedback]

Found something that's not documented properly, is unclear or even incorrect?

**Please share** your findings with us by contacting your PosAPI contact immediately - your support is highly appreciated!

**Thank you!**

## Global Error Codes [/GlobalErrorCodes]

These error-code combinations are shared accross different instances and modules of the PosAPI.

| Status Code | Error Code | Error Message                  |
|:-----------:|:----------:|--------------------------------|
| `400`       | `102`      | Invalid input                  |
| `400`       | `200`      | Invalid request message        |
| `400`       | `201`      | Invalid access security token  |
| `400`       | `204`      | Missing access security token  |
| `400`       | `205`      | Missing session security token |
| `400`       | `206`      | Missing user security   token  |
| `400`       | `207`      | Session security token expired |
| `400`       | `208`      | User security token expired    |
| `400`       | `209`      | Invalid session security token |
| `400`       | `210`      | Invalid user security token    |
| `403`       | `100`      | Forbidden                      |
| `403`       | `202`      | Module is blocked              |
| `403`       | `203`      | Request limit exceeded         |
| `403`       | `204`      | Missing access security token  |
| `403`       | `205`      | Missing session security token |
| `403`       | `206`      | Missing user security token    |
| `404`       | `103`      | Not found                      |
| `406`       | `104`      | Unsupported response format    |
| `415`       | `105`      | Unsupported request format     |
| `500`       | `101`      | Internal error                 |
| `503`       | `429`      | The remote server returned with: 429 - too many requests |
| `504`       | `106`      | Timeout occured when calling upstream server or service  |

## References [/References]

This section contains references to books or articles that give further insights in some of the technical concepts used in this document. For some articles we have provided appropriate web-links that where valid at the time of writing. Due to the volatile nature of the WWW some of these links may no longer active when these lines are being read. We apologize for this inconvenience.

- [REST] Roy T. Fielding: [Architectural Styles and the Design of Network-based Software Architectures](http://www.ics.uci.edu/~fielding/pubs/dissertation/top.htm "Architectural Styles and the Design of Network-based Software Architectures"), 2000
- [RESTful WS] L. Richardson and Sam Ruby: "RESTful Web Services”", 2007, O’Reilly
- [RESTful .NET] Jon Flanders: "RESTful .NET", 2008, O’Reilly
- [RFC 2616] Roy T. Fielding et al: [Hyptertext Transfer Protocol – HTTP/1.1](http://www.w3.org/Protocols/rfc2616/rfc2616.html "Hyptertext Transfer Protocol – HTTP/1.1") 

## Glossary [/Glossary]

| Term          | Description                                                                                                                                                                                                                                                                                                                                                                                                                        |
|---------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `Agent`       | A client application that accesses the PosAPI.                                                                                                                                                                                                                                                                                                                                                                                     |
| `API`         | Application   Programming Interface                                                                                                                                                                                                                                                                                                                                                                                                |
| `REST`        | Representational State Transfer – architectural style (and principles and guidelines) that the World Wide Web is based upon. REST is described in the dissertation of Roy Fielding.                                                                                                                                                                                                                                                |
| `HTTP`        | HyperText Transfer Protocol – protocol that is used to transmit data in the World Wide Web. HTTP is a stateless protocol that uses readable text messages for request and response. Each request and response message consists of a header and a body. The body contains of text, or may be completely omitted. The header includes the                                                                                            |
| `HTTP Header` | Each request and each response include headers that include data that describe the request or the response in more detail. Header fields are used to give information about the requested/returned media type, the language. Custom header fields can be used to transmit data that is used recurrently. For instance, information about authenticated customers has to be transmitted in each request in a specific header field. |
| `HTTP Method` | The method that is used to transmit data. The REST definition                                                                                                                                                                                                                                                                                                                                                                      |
| `URI`         | Uniform Resource Identifier – a textual description that identifies a resource. In the context of this document we always mean the identification within the World Wide Web. In that case an Uri looks like: http://api.bwin.com/myservice.                                                                                                                                                                                        |
| `URL`         | Uniform Resource Locator – a specific variant of an URI that identifies and localizes a resource using a network protocol. In this document all referenced URIs are URLs that use HTTP as the network protocol.                                                                                                                                                                                                                    |

## Documentation Guideline [/Guideline]

> The following addresses developers who are actively contributing to the API and its documenation.

- Use a JSON formatter/validator for all JSON request and response samples, e.g. [http://jsonformatter.curiousconcept.com/](http://jsonformatter.curiousconcept.com/ "JsonFormatter")
- Use a Markdown table generator in order to easily transform tables into markdown sytax, e.g. [http://www.tablesgenerator.com/markdown_tables](http://www.tablesgenerator.com/markdown_tables "TablesGenerator.com")