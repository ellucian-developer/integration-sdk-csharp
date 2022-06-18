/*
 * ******************************************************************************
 *   Copyright  2020-2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client;
using Ellucian.Ethos.Integration.Service;

using Moq;
using Moq.Protected;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Test
{
    public class SampleTestDataRepository
    {
        #region Data for tests

        public static readonly string API_KEY = "5c9735e0-bb89-49fa-80e9-41f5e48fe376";
        public static readonly string VERSION = "application/vnd.hedtech.integration.v7.2.0+json";

        public static string GetOneJsonRecordString()
        {
            return @"{
                'dateTime':'2020-09-01T12:00:00Z','severity':'Error','applicationName':'Colleague','responseCode':500,'description':'Internal Server Error',
                'details':'This is a more info on the info error','id':'b1a3fc8f-d0cd-4a8b-a6c6-af252f4e49f7','applicationId':'67b462f2-c554-4c15-91fa-e1194a85553b',
                'correlationId':'2468UserMade3242134','applicationSubtype':'EMA','resource':{'id':'b1a3fc8f-d0cd-4a8b-a6c6-af252f4e49f7','name':'persons'},
                'request':{'URI':'www.papa-johns.com','payload':'Order me a pizza','headers':['contentType','secondHeader','third header']}}";
        }

        public static string GetArrayJsonRecordString()
        {
            return @"[{'code':'ATHL','title':'Athletes','description':'Athletes','id':'f43aa813-efaf-427c-afc6-d8daf352fb3b'},{'code':'FRAT','title':'Fraternity','description':'Fraternity',
                       'id':'d6dcbad9-dd1c-41ee-93cb-26749449070e'},{'code':'SORO','title':'Sorority','description':'Sorority','id':'362e8bb3-c822-4ce8-843b-0f3f66499c0c'},{'code':'ROTC','title':'ROTC Participants',
                       'description':'ROTC Participants','id':'d43fd919-8a05-4483-8d39-fd794d578beb'},{'code':'VETS','title':'Military Veterans','description':'Military Veterans','id':'1a19225a-508e-4bdc-84b8-92485c74cfe3'},
                      {'code':'APPR','title':'Apprentices','description':'Apprentices','id':'4ccb1f44-758c-4733-ac90-f3204a0d5230'},{'code':'FTBL','title':'IPEDS-Football','description':'IPEDS-Football',
                       'id':'3d58b4be-fd87-45c3-a864-3fc7e0969c86'},{'code':'BSKT','title':'IPEDS-Basketball','description':'IPEDS-Basketball','id':'b66e3138-6e85-4c4b-b1d6-70becb615473'},{'code':'BSBL','title':'IPEDS-Baseball',
                       'description':'IPEDS-Baseball','id':'d80c51cf-d83f-42bf-8caa-0eb3d3bf1332'},{'code':'TRCK','title':'IPEDS-Track/CrossCountry','description':'IPEDS-Track/CrossCountry',
                       'id':'839fb6c0-f476-4182-a2d3-e3e621a8d8f4'}]";
        }

        public static JArray GetArrayJsonRecordJArray()
        {
            var json = @"[{'code':'ATHL','title':'Athletes','description':'Athletes','id':'f43aa813-efaf-427c-afc6-d8daf352fb3b'},{'code':'FRAT','title':'Fraternity','description':'Fraternity',
                       'id':'d6dcbad9-dd1c-41ee-93cb-26749449070e'},{'code':'SORO','title':'Sorority','description':'Sorority','id':'362e8bb3-c822-4ce8-843b-0f3f66499c0c'},{'code':'ROTC','title':'ROTC Participants',
                       'description':'ROTC Participants','id':'d43fd919-8a05-4483-8d39-fd794d578beb'},{'code':'VETS','title':'Military Veterans','description':'Military Veterans','id':'1a19225a-508e-4bdc-84b8-92485c74cfe3'},
                      {'code':'APPR','title':'Apprentices','description':'Apprentices','id':'4ccb1f44-758c-4733-ac90-f3204a0d5230'},{'code':'FTBL','title':'IPEDS-Football','description':'IPEDS-Football',
                       'id':'3d58b4be-fd87-45c3-a864-3fc7e0969c86'},{'code':'BSKT','title':'IPEDS-Basketball','description':'IPEDS-Basketball','id':'b66e3138-6e85-4c4b-b1d6-70becb615473'},{'code':'BSBL','title':'IPEDS-Baseball',
                       'description':'IPEDS-Baseball','id':'d80c51cf-d83f-42bf-8caa-0eb3d3bf1332'},{'code':'TRCK','title':'IPEDS-Track/CrossCountry','description':'IPEDS-Track/CrossCountry',
                       'id':'839fb6c0-f476-4182-a2d3-e3e621a8d8f4'}]";
            return JsonConvert.DeserializeObject<JArray>( json );
        }
        public static string GetArrayJsonRecordQAPI()
        {
            return @"[{'code':'ATHL','title':'Athletes','description':'Athletes','id':'f43aa813-efaf-427c-afc6-d8daf352fb3b'},{'code':'FRAT','title':'Fraternity','description':'Fraternity',
                       'id':'d6dcbad9-dd1c-41ee-93cb-26749449070e'},{'code':'SORO','title':'Sorority','description':'Sorority','id':'362e8bb3-c822-4ce8-843b-0f3f66499c0c'},{'code':'ROTC','title':'ROTC Participants',
                       'description':'ROTC Participants','id':'d43fd919-8a05-4483-8d39-fd794d578beb'},{'code':'VETS','title':'Military Veterans','description':'Military Veterans','id':'1a19225a-508e-4bdc-84b8-92485c74cfe3'},
                      {'code':'APPR','title':'Apprentices','description':'Apprentices','id':'4ccb1f44-758c-4733-ac90-f3204a0d5230'},{'code':'FTBL','title':'IPEDS-Football','description':'IPEDS-Football',
                       'id':'3d58b4be-fd87-45c3-a864-3fc7e0969c86'},{'code':'BSKT','title':'IPEDS-Basketball','description':'IPEDS-Basketball','id':'b66e3138-6e85-4c4b-b1d6-70becb615473'},{'code':'BSBL','title':'IPEDS-Baseball',
                       'description':'IPEDS-Baseball','id':'d80c51cf-d83f-42bf-8caa-0eb3d3bf1332'},{'code':'TRCK','title':'IPEDS-Track/CrossCountry','description':'IPEDS-Track/CrossCountry',
                       'id':'839fb6c0-f476-4182-a2d3-e3e621a8d8f4'}]";
        }

        public static string GetErrorMessage()
        {
            return @"[{'id':'b29c5c1c-d9d8-4ce8-bb18-89748e13776f','dateTime':'2020-10-16T12:14:45.319Z','severity':'info','responseCode':200,'description':'This application is sending requests for section-registrations (v7) 
                        which is a deprecated version ','applicationId':'9e8aaa0b-db17-4b83-986f-540b367207bc','resource':{'name':'section-registrations (v7)'}},{'id':'04c75eee-9661-4182-b740-ded5bf49b522','dateTime':'2020-10-16T12:14:30.336Z',
                        'severity':'info','responseCode':200,'description':'This application is sending requests for section-registrations (v7) which is a deprecated version ','applicationId':'9e8aaa0b-db17-4b83-986f-540b367207bc',
                        'resource':{'name':'section-registrations (v7)'}},{'id':'77286385-4395-4e49-8ae5-4f447a99a367','dateTime':'2020-10-16T12:14:15.919Z','severity':'info','responseCode':200,
                        'description':'This application is sending requests for section-registrations (v7) which is a deprecated version ','applicationId':'9e8aaa0b-db17-4b83-986f-540b367207bc','resource':{'name':'section-registrations (v7)'}},
                        {'id':'b70064d0-f08a-4b8f-afaa-1bc40628c74c','dateTime':'2020-10-16T12:14:00.314Z','severity':'info','responseCode':200,'description':'This application is sending requests for section-registrations (v7) which is a deprecated version ',
                        'applicationId':'9e8aaa0b-db17-4b83-986f-540b367207bc','resource':{'name':'section-registrations (v7)'}},
                        {'id':'abfc46f2-3ab5-4d19-a3ef-68b2b902ad4b','dateTime':'2020-10-16T12:13:45.257Z','severity':'info','responseCode':200,'description':'This application is sending requests for section-registrations (v7) which is a deprecated version ',
                        'applicationId':'9e8aaa0b-db17-4b83-986f-540b367207bc','resource':{'name':'section-registrations (v7)'}},{'id':'a42534d2-79e6-44cd-ac33-72a604fd7371','dateTime':'2020-10-16T12:13:30.350Z','severity':'info','responseCode':200,'description':'This application is sending requests for section-registrations (v7) which is a deprecated version ','applicationId':'9e8aaa0b-db17-4b83-986f-540b367207bc',
                        'resource':{'name':'section-registrations (v7)'}},{'id':'6a587f86-e1e6-41e0-8514-60d8e07ddcd5','dateTime':'2020-10-16T12:13:16.471Z','severity':'info','responseCode':200,'description':'This application is sending requests for section-registrations (v7) which is a deprecated version ','applicationId':'9e8aaa0b-db17-4b83-986f-540b367207bc','resource':{'name':'section-registrations (v7)'}},
                        {'id':'cc93f403-d7dc-421f-8fd8-55529d9f6138','dateTime':'2020-10-16T12:13:00.910Z','severity':'info','responseCode':200,'description':'This application is sending requests for section-registrations (v7) which is a deprecated version ','applicationId':'9e8aaa0b-db17-4b83-986f-540b367207bc','resource':{'name':'section-registrations (v7)'}},{'id':'68c7fd69-6e23-4bac-8d34-d337247eb392','dateTime':'2020-10-16T12:12:45.403Z','severity':'info','responseCode':200,'description':'This application is sending requests for section-registrations (v7) which is a deprecated version ','applicationId':'9e8aaa0b-db17-4b83-986f-540b367207bc','resource':{'name':'section-registrations (v7)'}},{'id':'6c16947c-cdac-4cad-b10c-a06f3250e35d','dateTime':'2020-10-16T12:12:30.908Z','severity':'info','responseCode':200,'description':'This application is sending requests for section-registrations (v7) which is a deprecated version ','applicationId':'9e8aaa0b-db17-4b83-986f-540b367207bc','resource':{'name':'section-registrations (v7)'}}]";
        }

        public static string GetToken()
        {
            return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI0MzczOWZiYi01MTE4LTQwMDktYmU0OS01MGNhN2Y5MDcwY2IiLCJ0b2ciOltdLCJ0ZW5hbnQiOnsiaWQiOiJiYmMwYTEzZi00MjgxLTQ3ODEtYjRmYS0zZWIwZWRmMDQ1MTciLCJhY2NvdW50SWQiOiJJbnRlcm5hbEJyYW5kb25NY0ZhcmxhbmQiLCJhbGlhcyI6ImJ0ZXN0OTUiLCJuYW1lIjoiQnJhbmRvbiIsImxhYmVsIjoiVGVzdCJ9LCJpYXQiOjE2MDI5ODQzNDQsImV4cCI6MTYwMjk4NDY0NH0.dsjiXHaS50qDoQW4HX4pKlxnds65KbnOLIXsQCIJgIU";
        }

        public static string GetAvailableResourcesData()
        {
            return @"[{'id':'eef6b098-17fe-4d9e-b8f8-5d949420ffa6','name':'Auth App 1','resources':[{'name':'address-types','representations':[{'X-Media-Type':'application/vnd.hedtech.integration.v6+json',
                    'methods':['get'],'version':'v6'},{'X-Media-Type':'application/vnd.hedtech.integration.v6.1.0+json','methods':['get'],'version':'v6.1.0'},{'X-Media-Type':'application/json',
                    'methods':['get']}]},{'name':'buildings','representations':[{'X-Media-Type':'application/vnd.hedtech.integration.v6+json','methods':['get'],'version':'v6'},
                    {'X-Media-Type':'application/json','methods':['get']}]}]},{'id':'2f977334-edfd-4408-a227-21663664abc9','name':'Auth App 2','resources':[{'name':'buildings',
                    'representations':[{'X-Media-Type':'application/vnd.hedtech.integration.v6+json','methods':['get'],'version':'v6'},{'X-Media-Type':'application/json','methods':['get']}]},
                    {'name':'course-levels','representations':[{'X-Media-Type':'application/vnd.hedtech.integration.v6+json','methods':['get'],'version':'v6'},{'X-Media-Type':'application/json',
                    'methods':['get']}]}]}]";
        }

        public static string GetResourceForPersons()
        {
            return @"[{'id':'b7bc3d67-5d69-4191-9744-36eb1eb4ba72','name':'Banner Integration API','about':[{'applicationName':'IntegrationApi','applicationVersion':'9.22','buildNumber':'7','name':'IntegrationApi','version':'9.22'}],'resources':[{'name':'persons','representations':[{'filters':['title','firstName','middleName','lastNamePrefix','lastName','pedigree','role','credentialType','credentialValue','personFilter'],'X-Media-Type':'application/vnd.hedtech.integration.v6+json','methods':['get','post','put'],'version':'v6'},{'filters':['names.title','names.firstName','names.middleName','names.lastNamePrefix','names.lastName','names.pedigree','roles.role','credential.type','credential.value','personFilter'],'X-Media-Type':'application/vnd.hedtech.integration.v8+json','methods':['get','post','put'],'version':'v8'},{'filters':['names.title','names.firstName','names.middleName','names.lastNamePrefix','names.lastName','names.pedigree','roles.role','credentials.type','credentials.value','alternativeCredentials.type.id','alternativeCredentials.value','emails.address'],'getAllPatterns':[{'X-Media-Type':'application/vnd.hedtech.integration.v12+json','method':'get','name':'paging'},{'X-Media-Type':'application/vnd.hedtech.integration.bulk-requests.v1+json','method':'post','name':'batch'}],'X-Media-Type':'application/vnd.hedtech.integration.v12+json','methods':['get','post','put'],'namedQueries':[{'filters':['personFilter'],'name':'personFilter'}],'version':'v12'},{'filters':['names.title','names.firstName','names.middleName','names.lastNamePrefix','names.lastName','names.pedigree','roles.role','credentials.type','credentials.value','alternativeCredentials.type.id','alternativeCredentials.value','emails.address'],'getAllPatterns':[{'X-Media-Type':'application/vnd.hedtech.integration.v12+json','method':'get','name':'paging'},{'X-Media-Type':'application/vnd.hedtech.integration.bulk-requests.v1+json','method':'post','name':'batch'}],'X-Media-Type':'application/vnd.hedtech.integration.v12.0.0+json','methods':['get','post','put'],'namedQueries':[{'filters':['personFilter'],'name':'personFilter'}],'version':'v12.0.0'},{'filters':['names.title','names.firstName','names.middleName','names.lastNamePrefix','names.lastName','names.pedigree','roles.role','credentials.type','credentials.value','alternativeCredentials.type.id','alternativeCredentials.value','emails.address'],'getAllPatterns':[{'X-Media-Type':'application/vnd.hedtech.integration.v12+json','method':'get','name':'paging'},{'X-Media-Type':'application/vnd.hedtech.integration.bulk-requests.v1+json','method':'post','name':'batch'}],'X-Media-Type':'application/vnd.hedtech.integration.v12.1.0+json','methods':['get','post','put'],'namedQueries':[{'filters':['personFilter'],'name':'personFilter'}],'version':'v12.1.0'},{'filters':['names.title','names.firstName','names.middleName','names.lastNamePrefix','names.lastName','names.pedigree','roles.role','credentials.type','credentials.value','alternativeCredentials.type.id','alternativeCredentials.value','emails.address'],'getAllPatterns':[{'X-Media-Type':'application/vnd.hedtech.integration.v12+json','method':'get','name':'paging'},{'X-Media-Type':'application/vnd.hedtech.integration.bulk-requests.v1+json','method':'post','name':'batch'}],'X-Media-Type':'application/vnd.hedtech.integration.v12.2.0+json','methods':['get','post','put'],'namedQueries':[{'filters':['personFilter'],'name':'personFilter'}],'version':'v12.2.0'},{'filters':['names.title','names.firstName','names.middleName','names.lastNamePrefix','names.lastName','names.pedigree','roles.role','credentials.type','credentials.value','alternativeCredentials.type.id','alternativeCredentials.value','emails.address'],'getAllPatterns':[{'X-Media-Type':'application/vnd.hedtech.integration.v12+json','method':'get','name':'paging'},{'X-Media-Type':'application/vnd.hedtech.integration.bulk-requests.v1+json','method':'post','name':'batch'}],'X-Media-Type':'application/vnd.hedtech.integration.v12.3.0+json','methods':['get','post','put'],'namedQueries':[{'filters':['personFilter'],'name':'personFilter'}],'version':'v12.3.0'},{'filters':['names.title','names.firstName','names.middleName','names.lastNamePrefix','names.lastName','names.pedigree','roles.role','credentials.type','credentials.value','alternativeCredentials.type.id','alternativeCredentials.value','emails.address'],'getAllPatterns':[{'X-Media-Type':'application/vnd.hedtech.integration.v12+json','method':'get','name':'paging'},{'X-Media-Type':'application/vnd.hedtech.integration.bulk-requests.v1+json','method':'post','name':'batch'}],'X-Media-Type':'application/json','methods':['get','post','put'],'namedQueries':[{'filters':['personFilter'],'name':'personFilter'}]},{'X-Media-Type':'application/vnd.hedtech.integration.bulk-requests.v1.0.0+json','methods':['get','post'],'version':'v1.0.0'},{'X-Media-Type':'application/vnd.hedtech.integration.bulk-requests.v1+json','methods':['get','post'],'version':'v1'}]}]}]";
        }

        public static string GetAvailableResourcesWithFiltersAndNamedQueriesData()
        {
            return @"[{'appId':'b7bc3d67-5d69-4191-9744-36eb1eb4ba72','appName':'BannerIntegrationAPI','resource':{'name':'persons','representations':[{'filters':['title','firstName','middleName','lastNamePrefix',
                    'lastName','pedigree','role','credentialType','credentialValue','personFilter'],'X-Media-Type':'application/vnd.hedtech.integration.v6+json','methods':['get','post','put'],'version':'v6'},
                    {'filters':['names.title','names.firstName','names.middleName','names.lastNamePrefix','names.lastName','names.pedigree','roles.role','credential.type','credential.value','personFilter'],
                    'X-Media-Type':'application/vnd.hedtech.integration.v8+json','methods':['get','post','put'],'version':'v8'},{'filters':['names.title','names.firstName','names.middleName','names.lastNamePrefix',
                    'names.lastName','names.pedigree','roles.role','credentials.type','credentials.value','alternativeCredentials.type.id','alternativeCredentials.value','emails.address'],
                    'getAllPatterns':[{'X-Media-Type':'application/vnd.hedtech.integration.v12+json','method':'get','name':'paging'},{'X-Media-Type':'application/vnd.hedtech.integration.bulk-requests.v1+json',
                    'method':'post','name':'batch'}],'X-Media-Type':'application/vnd.hedtech.integration.v12+json','methods':['get','post','put'],'namedQueries':[{'filters':['personFilter'],'name':'personFilter'}],
                    'version':'v12'},{'filters':['names.title','names.firstName','names.middleName','names.lastNamePrefix','names.lastName','names.pedigree','roles.role','credentials.type','credentials.value',
                    'alternativeCredentials.type.id','alternativeCredentials.value','emails.address'],'getAllPatterns':[{'X-Media-Type':'application/vnd.hedtech.integration.v12+json','method':'get','name':'paging'},
                    {'X-Media-Type':'application/vnd.hedtech.integration.bulk-requests.v1+json','method':'post','name':'batch'}],'X-Media-Type':'application/vnd.hedtech.integration.v12.0.0+json',
                    'methods':['get','post','put'],'namedQueries':[{'filters':['personFilter'],'name':'personFilter'}],'version':'v12.0.0'},{'filters':['names.title','names.firstName','names.middleName',
                    'names.lastNamePrefix','names.lastName','names.pedigree','roles.role','credentials.type','credentials.value','alternativeCredentials.type.id','alternativeCredentials.value','emails.address'],
                    'getAllPatterns':[{'X-Media-Type':'application/vnd.hedtech.integration.v12+json','method':'get','name':'paging'},{'X-Media-Type':'application/vnd.hedtech.integration.bulk-requests.v1+json',
                    'method':'post','name':'batch'}],'X-Media-Type':'application/vnd.hedtech.integration.v12.1.0+json','methods':['get','post','put'],'namedQueries':[{'filters':['personFilter'],'name':'personFilter'}],
                    'version':'v12.1.0'},{'filters':['names.title','names.firstName','names.middleName','names.lastNamePrefix','names.lastName','names.pedigree','roles.role','credentials.type','credentials.value',
                    'alternativeCredentials.type.id','alternativeCredentials.value','emails.address'],'getAllPatterns':[{'X-Media-Type':'application/vnd.hedtech.integration.v12+json','method':'get','name':'paging'},
                    {'X-Media-Type':'application/vnd.hedtech.integration.bulk-requests.v1+json','method':'post','name':'batch'}],'X-Media-Type':'application/vnd.hedtech.integration.v12.2.0+json',
                    'methods':['get','post','put'],'namedQueries':[{'filters':['personFilter'],'name':'personFilter'}],'version':'v12.2.0'},{'filters':['names.title','names.firstName','names.middleName',
                    'names.lastNamePrefix','names.lastName','names.pedigree','roles.role','credentials.type','credentials.value','alternativeCredentials.type.id','alternativeCredentials.value','emails.address'],
                    'getAllPatterns':[{'X-Media-Type':'application/vnd.hedtech.integration.v12+json','method':'get','name':'paging'},{'X-Media-Type':'application/vnd.hedtech.integration.bulk-requests.v1+json',
                    'method':'post','name':'batch'}],'X-Media-Type':'application/vnd.hedtech.integration.v12.3.0+json','methods':['get','post','put'],'namedQueries':[{'filters':['personFilter'],'name':'personFilter'}],
                    'version':'v12.3.0'},{'filters':['names.title','names.firstName','names.middleName','names.lastNamePrefix','names.lastName','names.pedigree','roles.role','credentials.type','credentials.value',
                    'alternativeCredentials.type.id','alternativeCredentials.value','emails.address'],'getAllPatterns':[{'X-Media-Type':'application/vnd.hedtech.integration.v12+json','method':'get','name':'paging'},
                    {'X-Media-Type':'application/vnd.hedtech.integration.bulk-requests.v1+json','method':'post','name':'batch'}],'X-Media-Type':'application/json','methods':['get','post','put'],
                    'namedQueries':[{'filters':['personFilter'],'name':'personFilter'}]},{'X-Media-Type':'application/vnd.hedtech.integration.bulk-requests.v1.0.0+json','methods':['get','post'],'version':'v1.0.0'},
                    {'X-Media-Type':'application/vnd.hedtech.integration.bulk-requests.v1+json','methods':['get','post'],'version':'v1'}]}}]";
        }

        public static string GetChangeNotificationSingleJson()
        {
            return @"{'id':'55840','published':'2017-12-12 22:37:44.242116+00','resource':{'name':'accounting-string-component-values','id':'f15a95e1-d12f-4833-8cfa-6cbe7995d1bb',
                    'version':'application/vnd.hedtech.integration.v11+json','domain':'https://somedomain.com'},'operation':'replaced','contentType':'resource-representation',
                    'content':{'value':'11-00-01-80-20511-52011','description':'Staff Wages Part Time : Fadda 2 Research Grant','component':{'id':'76fc67b1-da04-4fa4-8eec-2ced9880f206'},
                    'transactionStatus':'available','type':{'account':'expense'},'metadata':{},'id':'f15a95e1-d12f-4833-8cfa-6cbe7995d1bb'},'publisher':{'id':'853d32d4-412b-4518-9d70-a7ba8b7e7ce4',
                    'applicationName':'Colleague Main Authoritative Source','tenant':{'id':'123','alias':'alias','name':'Some Name','environment':'Env123'}}}";
        }

        public static string GetMockSingleProxyRecord()
        {
            return @"{'value':'EJR-MUSC','description':'Eleanor's Music Project','component':{'id':'77f64fef-5869-49e1-8dca-756c2584331c'},'transactionStatus':'available','type':{'account':'expense'},
                      'effectiveStartOn':'2013-03-18T00:00:00','effectiveEndOn':'2014-06-30T00:00:00','status':'active','grants':[{'id':'1e7750fe-fcae-4d2a-9412-296c9ae3526f'}],
                      'id':'f15a95e1-d12f-4833-8cfa-6cbe7995d1bb'}";
        }

        public static string GetChangeNotificationArrayJson()
        {
            return @"[{'id':'55840','published':'2017-12-12 22:37:44.242116+00','resource':{'name':'accounting-string-component-values','id':'f15a95e1-d12f-4833-8cfa-6cbe7995d1bb',
                    'version':'application/vnd.hedtech.integration.v11+json'},'operation':'replaced','contentType':'resource-representation','content':{'value':'11-00-01-80-20511-52011',
                    'description':'Staff Wages Part Time : Fadda 2 Research Grant','component':{'id':'76fc67b1-da04-4fa4-8eec-2ced9880f206'},'transactionStatus':'available',
                    'type':{'account':'expense'},'metadata':{},'id':'f15a95e1-d12f-4833-8cfa-6cbe7995d1bb'},'publisher':{'id':'853d32d4-412b-4518-9d70-a7ba8b7e7ce4',
                    'applicationName':'Colleague Main Authoritative Source'}},{'id':'55841','published':'2017-12-12 22:37:44.242116+00','resource':{'name':'accounting-string-component-values',
                    'id':'f15a95e1-d12f-4833-8cfa-6cbe7995d1bb','version':'application/vnd.hedtech.integration.v11+json'},'operation':'replaced','contentType':'resource-representation',
                    'content':{'value':'11-00-01-80-20511-52011','description':'Staff Wages Part Time : Fadda 2 Research Grant','component':{'id':'76fc67b1-da04-4fa4-8eec-2ced9880f206'},
                    'transactionStatus':'available','type':{'account':'expense'},'metadata':{},'id':'f15a95e1-d12f-4833-8cfa-6cbe7995d1bb'},'publisher':{'id':'853d32d4-412b-4518-9d70-a7ba8b7e7ce4',
                    'applicationName':'Colleague Main Authoritative Source'}}]";
        }

        public static string GetAppConfig()
        {
            return @"{'id':'eef6b098-17fe-4d9e-b8f8-5d949420ffa6','name':'client app','subscriptions':[{'resourceName':'address-types','applicationId':'eef6b098-17fe-4d9e-b8f8-5d949420ffa6'}],
                      'ownerOverrides':[{'resourceName':'buildings','applicationId':'eef6b098-17fe-4d9e-b8f8-5d949420ffa6'},{'resourceName':'address-types','applicationId':'eef6b098-17fe-4d9e-b8f8-5d949420ffa6'}],
                      'metadata':{'createdBy':'bmcfarland@ellucian.me','createdOn':'2020-07-21T17:58:59.88Z','modifiedBy':'bmcfarland@ellucian.me','modifiedOn':'2020-10-27T16:28:34.979Z','version':'4.9.2'}}";
        }

        public static string GetSingleForStronglyTyped()
        {
            return @"{'id':'eef6b098-17fe-4d9e-b8f8-5d949420ffa6','description':'SomeDescription'}";
        }
        public static string GetArrayForStronglyTyped()
        {
            return @"[{'id':'eef6b098-17fe-4d9e-b8f8-5d949420ffa6','description':'SomeDescription'}]";
        }

        #endregion

        #region Mocks
        public static HttpClient GetMockHttpClientWithSingleRecordForStronglyTyped()
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent( GetSingleForStronglyTyped() ),
                RequestMessage = new HttpRequestMessage() { RequestUri = new Uri( "https://integrate.elluciancloud.com/api/student-cohorts" ) }
            };
            //setup mock
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            //Setup sequence
            mockHttpMessageHandler.Protected()
               .SetupSequence<Task<HttpResponseMessage>>(
               "SendAsync",
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>() )
               .ReturnsAsync( new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent( GetToken() ) } )
               .ReturnsAsync( responseMessage );

            HttpClient httpClient;
            httpClient = new HttpClient( mockHttpMessageHandler.Object );
            httpClient.DefaultRequestHeaders.Add( "pragma", "no-cache" );
            httpClient.DefaultRequestHeaders.Add( "cache-control", "no-cache" );
            httpClient.DefaultRequestHeaders.Add( "User-Agent", "EllucianEthosIntegrationSdk" );
            httpClient.Timeout = new TimeSpan( 0, 0, 0, 60000, 0 );

            return httpClient;
        }

        public static HttpClient GetMockHttpClient()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            HttpClient httpClient;
            dict.Add( "Authorization", "Bearer 1234" );
            //setup mock
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            //Setup sequence
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>() )
                .ReturnsAsync( new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent( GetToken() ) } );
            httpClient = new HttpClient( mockHttpMessageHandler.Object );
            httpClient.DefaultRequestHeaders.Add( "pragma", "no-cache" );
            httpClient.DefaultRequestHeaders.Add( "cache-control", "no-cache" );
            httpClient.DefaultRequestHeaders.Add( "User-Agent", "EllucianEthosIntegrationSdk" );
            httpClient.Timeout = new TimeSpan( 0, 0, 0, 60000, 0 );

            return httpClient;
        }

        public static HttpClient GetMockHttpClientForEthosChangeNotificationService()
        {
            HttpResponseMessage authMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent( GetToken() ),
                RequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri( "https://integrate.elluciancloud.com/auth?expirationMinutes=60" )
                }
            };

            HttpResponseMessage cnRresponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent( GetChangeNotificationArrayJson() ),
                RequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri( "https://integrate.elluciancloud.com/consume" )
                }
            };
            cnRresponseMessage.Headers.Add( "x-remaining", "2" );

            HttpResponseMessage proxyResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent( GetMockSingleProxyRecord() ),
                RequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri( "https://integrate.elluciancloud.com/api/accounting-string-component-values/f15a95e1-d12f-4833-8cfa-6cbe7995d1bb" )
                }
            };
            proxyResponseMessage.Headers.Add( "x-media-type", "application/vnd.hedtech.integration.v8+json" );

            //setup mock
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>( MockBehavior.Strict );
            mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>() )
                .ReturnsAsync( authMessage )
                .ReturnsAsync( cnRresponseMessage )
                .ReturnsAsync( proxyResponseMessage );

            HttpClient httpClient = new HttpClient( mockHttpMessageHandler.Object );
            httpClient.DefaultRequestHeaders.Add( "pragma", "no-cache" );
            httpClient.DefaultRequestHeaders.Add( "cache-control", "no-cache" );
            httpClient.DefaultRequestHeaders.Add( "User-Agent", "EllucianEthosIntegrationSdk" );
            httpClient.Timeout = new TimeSpan( 0, 0, 0, 60000, 0 );

            return httpClient;
        }

        public static EthosChangeNotificationService GetMockEthosChangeNotificationService()
        {
            EthosChangeNotificationService service;
            var mockHttpClient = GetMockHttpClientForEthosChangeNotificationService();

            Mock<IHttpProtocolClientBuilder> httpBuilder = new Mock<IHttpProtocolClientBuilder>();
            httpBuilder.Setup( h => h.Client ).Returns( mockHttpClient );

            EthosClientBuilder ethosClientBuilder = new EthosClientBuilder( API_KEY );
            var builder = ethosClientBuilder.GetType().GetField( "builder", BindingFlags.NonPublic | BindingFlags.Instance );
            builder.SetValue( ethosClientBuilder, httpBuilder.Object );

            return service = EthosChangeNotificationService.Build( a =>
            {
                a.WithEthosClientBuilder( ethosClientBuilder )
                 .WithConnectionTimeout( 10 );
            }, API_KEY );
        }

        public static (Dictionary<string, string> dict, HttpClient httpClient) GetMockSequenceForErrorClientWithOK()
        {
            Mock<IHttpProtocolClientBuilder> builder = new Mock<IHttpProtocolClientBuilder>();
            builder.Object.BuildHttpClient( null );

            Dictionary<string, string> dict = new Dictionary<string, string>();
            HttpClient httpClient;
            dict.Add( "Authorization", "Bearer 1234" );
            //setup mock
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            //Setup sequence
            mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>() )
                .ReturnsAsync( new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent( GetToken() ) } )
                .ReturnsAsync( new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent( GetErrorMessage() ) } );
            httpClient = new HttpClient( mockHttpMessageHandler.Object );
            return (dict, httpClient);
        }
        public static (Dictionary<string, string> dict, HttpClient httpClient) GetMockClientWithArrayWithOKStatus()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            HttpClient httpClient;
            dict.Add( "Authorization", "Bearer 1234" );
            HttpResponseMessage responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent( GetArrayJsonRecordString() ),
                RequestMessage = new HttpRequestMessage() { RequestUri = new Uri( "https://integrate.elluciancloud.com/api/student-cohorts" ) }
            };
            responseMessage.Headers.Add( "Date", DateTimeOffset.Now.ToString( "ddd, dd MMM yyyy HH:mm:ss 'GMT'" ) );
            responseMessage.Headers.Add( "x-total-count", "10" );
            responseMessage.Headers.Add( "x-application-context", "application:production:8092" );
            responseMessage.Headers.Add( "x-max-page-size", "500" );
            responseMessage.Headers.Add( "x-media-type", VERSION );
            responseMessage.Headers.Add( "hedtech-ethos-integration-application-id", "15f97b04-d893-4428-a5b9-81bc997e6493" );
            responseMessage.Headers.Add( "hedtech-ethos-integration-application-name", "Banner Student API" );


            //setup mock
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>() )
                .ReturnsAsync( responseMessage );

            httpClient = new HttpClient( mockHttpMessageHandler.Object );
            return (dict, httpClient);
        }

        public static (Dictionary<string, string> dict, HttpClient httpClient) GetMockSequenceForEthosProxyClientWithOK()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            HttpClient httpClient;
            dict.Add( "Authorization", "Bearer 1234" );
            //setup mock
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            HttpResponseMessage responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent( GetArrayJsonRecordString() ),
                RequestMessage = new HttpRequestMessage() { RequestUri = new Uri( "https://integrate.elluciancloud.com/api/student-cohorts" ) }
            };
            responseMessage.Headers.Add( "Date", DateTimeOffset.Now.ToString( "ddd, dd MMM yyyy HH:mm:ss 'GMT'" ) );
            responseMessage.Headers.Add( "x-total-count", "10" );
            responseMessage.Headers.Add( "x-application-context", "application:production:8092" );
            responseMessage.Headers.Add( "x-max-page-size", "500" );
            responseMessage.Headers.Add( "x-media-type", VERSION );
            responseMessage.Headers.Add( "hedtech-ethos-integration-application-id", "15f97b04-d893-4428-a5b9-81bc997e6493" );
            responseMessage.Headers.Add( "hedtech-ethos-integration-application-name", "Banner Student API" );
            //Setup sequence
            mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>() )
                .ReturnsAsync( new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent( GetToken() ) } )
                .ReturnsAsync( responseMessage );
            httpClient = new HttpClient( mockHttpMessageHandler.Object );
            return (dict, httpClient);
        }

        public static (Dictionary<string, string> dict, HttpClient httpClient) GetMockSequenceForEthosFilterQueryClientWithOK( string filter )
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            HttpClient httpClient;
            dict.Add( "Authorization", "Bearer 1234" );
            //setup mock
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            HttpResponseMessage responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent( GetArrayJsonRecordString() ),
                RequestMessage = new HttpRequestMessage() { RequestUri = new Uri( $"https://integrate.elluciancloud.com/api/student-cohorts{filter}" ) }
            };
            responseMessage.Headers.Add( "Date", DateTimeOffset.Now.ToString( "ddd, dd MMM yyyy HH:mm:ss 'GMT'" ) );
            responseMessage.Headers.Add( "x-total-count", "10" );
            responseMessage.Headers.Add( "x-application-context", "application:production:8092" );
            responseMessage.Headers.Add( "x-max-page-size", "500" );
            responseMessage.Headers.Add( "x-media-type", VERSION );
            responseMessage.Headers.Add( "hedtech-ethos-integration-application-id", "15f97b04-d893-4428-a5b9-81bc997e6493" );
            responseMessage.Headers.Add( "hedtech-ethos-integration-application-name", "Banner Student API" );
            //Setup sequence
            mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>() )
                .ReturnsAsync( new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent( GetToken() ) } )
                .ReturnsAsync( responseMessage )
                .ReturnsAsync( responseMessage )
                .ReturnsAsync( responseMessage );
            httpClient = new HttpClient( mockHttpMessageHandler.Object );
            return (dict, httpClient);
        }

        public static HttpClient GetMockSequenceForEthosQAPIClientWithOK( string filter )
        {
            HttpClient httpClient;
            //setup mock
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            HttpResponseMessage responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent( GetArrayJsonRecordQAPI() ),
                RequestMessage = new HttpRequestMessage() { RequestUri = new Uri( $"https://integrate.elluciancloud.com/api/student-cohorts{filter}" ) }
            };
            responseMessage.Headers.Add( "x-total-count", "10" );

            //Setup sequence
            mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>() )
                .ReturnsAsync( new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent( GetToken() ) } )
                .ReturnsAsync( responseMessage )
                .ReturnsAsync( responseMessage )
                .ReturnsAsync( responseMessage );
            httpClient = new HttpClient( mockHttpMessageHandler.Object );
            return httpClient;
        }

        public static (Dictionary<string, string> dict, HttpClient httpClient) GetMockSequenceForEthosFilterQueryClientWithFilterMap()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            HttpClient httpClient;
            dict.Add( "Authorization", "Bearer 1234" );
            //setup mock
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            HttpResponseMessage responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent( GetArrayJsonRecordString() ),
                RequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri( $"https://integrate.elluciancloud.com/api/student-cohorts?firstName=FIRST_NAME" )
                }
            };
            responseMessage.Headers.Add( "Date", DateTimeOffset.Now.ToString( "ddd, dd MMM yyyy HH:mm:ss 'GMT'" ) );
            responseMessage.Headers.Add( "x-total-count", "10" );
            responseMessage.Headers.Add( "x-application-context", "application:production:8092" );
            responseMessage.Headers.Add( "x-max-page-size", "500" );
            responseMessage.Headers.Add( "x-media-type", VERSION );
            responseMessage.Headers.Add( "hedtech-ethos-integration-application-id", "15f97b04-d893-4428-a5b9-81bc997e6493" );
            responseMessage.Headers.Add( "hedtech-ethos-integration-application-name", "Banner Student API" );
            //Setup sequence
            mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>() )
                .ReturnsAsync( new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent( GetToken() ) } )
                .ReturnsAsync( responseMessage );
            httpClient = new HttpClient( mockHttpMessageHandler.Object );
            return (dict, httpClient);
        }

        public static (Dictionary<string, string> dict, HttpClient httpClient) GetMockSequenceForEthosProxyClientWithOKForPaging()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            HttpClient httpClient;
            dict.Add( "Authorization", "Bearer 1234" );
            //setup mock
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            HttpResponseMessage responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent( GetArrayJsonRecordString() ),
                RequestMessage = new HttpRequestMessage() { RequestUri = new Uri( "https://integrate.elluciancloud.com/api/student-cohorts" ) }
            };
            responseMessage.Headers.Add( "Date", DateTimeOffset.Now.ToString( "ddd, dd MMM yyyy HH:mm:ss 'GMT'" ) );
            responseMessage.Headers.Add( "x-total-count", "10" );
            responseMessage.Headers.Add( "x-application-context", "application:production:8092" );
            responseMessage.Headers.Add( "x-max-page-size", "500" );
            responseMessage.Headers.Add( "x-media-type", VERSION );
            responseMessage.Headers.Add( "hedtech-ethos-integration-application-id", "15f97b04-d893-4428-a5b9-81bc997e6493" );
            responseMessage.Headers.Add( "hedtech-ethos-integration-application-name", "Banner Student API" );
            //Setup sequence
            mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>() )
                .ReturnsAsync( new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent( GetToken() ) } )
                .ReturnsAsync( responseMessage )
                .ReturnsAsync( responseMessage )
                .ReturnsAsync( responseMessage )
                .ReturnsAsync( responseMessage )
                .ReturnsAsync( responseMessage )
                .ReturnsAsync( responseMessage );
            httpClient = new HttpClient( mockHttpMessageHandler.Object );
            return (dict, httpClient);
        }
        public static (Dictionary<string, string> dict, HttpClient httpClient) GetMockClientWithOKSingleRecordWithAccessToken()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            HttpClient httpClient;
            dict.Add( "Authorization", "Bearer 1234" );
            HttpResponseMessage responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent( GetOneJsonRecordString() ),
                RequestMessage = new HttpRequestMessage() { RequestUri = new Uri( "https://integrate.elluciancloud.com/api/student-cohorts" ) }
            };
            responseMessage.Headers.Add( "Date", DateTimeOffset.Now.ToString( "ddd, dd MMM yyyy HH:mm:ss 'GMT'" ) );
            responseMessage.Headers.Add( "x-total-count", "10" );
            responseMessage.Headers.Add( "x-media-type", VERSION );
            responseMessage.Headers.Add( "hedtech-ethos-integration-application-name", "Banner Student API" );
            //setup mock
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>() )
                .ReturnsAsync( new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent( GetToken() ) } )
                .ReturnsAsync( responseMessage );

            httpClient = new HttpClient( mockHttpMessageHandler.Object );
            return (dict, httpClient);
        }
        public static (Dictionary<string, string> dict, HttpClient httpClient) GetMockClientWithOK()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            HttpClient httpClient;
            dict.Add( "Authorization", $"Bearer { API_KEY }" );
            //setup mock
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>() )
                .ReturnsAsync( new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent( GetToken() ) } )
                .ReturnsAsync( new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent( "[{'id':1,'value':'1'}]" ),
                    RequestMessage = new HttpRequestMessage() { RequestUri = new Uri( "https://integrate.elluciancloud.com/api/student-cohorts" ) }
                } );

            httpClient = new HttpClient( mockHttpMessageHandler.Object );
            return (dict, httpClient);
        }
        public static (Dictionary<string, string> dict, HttpClient httpClient) GetMockErrorMessageClientWithOK( bool multiple )
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            HttpClient httpClient;
            dict.Add( "Authorization", "Bearer 1234" );
            HttpResponseMessage response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent( GetErrorMessage() ),
                RequestMessage = new HttpRequestMessage() { RequestUri = new Uri( "https://integrate.elluciancloud.com/errors" ) }
            };
            if ( !multiple )
            {
                response.Content = new StringContent( GetOneJsonRecordString() );
            }

            response.Headers.Add( "x-total-count", "10" );
            //setup mock
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>() )
                .ReturnsAsync( new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent( GetToken() ) } )
                .ReturnsAsync( response )
                .ReturnsAsync( response )
                .ReturnsAsync( response )
                .ReturnsAsync( response )
                .ReturnsAsync( response );

            httpClient = new HttpClient( mockHttpMessageHandler.Object );
            return (dict, httpClient);
        }
        public static (Dictionary<string, string> dict, HttpClient httpClient) GetMockClientWithNotFound()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            HttpClient httpClient;
            dict.Add( "Authorization", $"Bearer { API_KEY }" );
            //setup mock
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>() )
                .ReturnsAsync( new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent( GetToken() ) } )
                .ReturnsAsync( new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound, Content = new StringContent( "{'errors':[{'code':'Global.SchemaValidation.Error','description':'Errors parsing input JSON.','message':'Student Cohort not found'}]}" ) } );

            httpClient = new HttpClient( mockHttpMessageHandler.Object );
            return (dict, httpClient);
        }
        public static (Dictionary<string, string> dict, HttpClient httpClient) GetMockEthosMessagesClientWithArrayWithOKStatus()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            HttpClient httpClient;
            dict.Add( "Authorization", GetToken() );
            HttpResponseMessage responseMessage = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent( GetChangeNotificationArrayJson() ) };
            responseMessage.Headers.Add( "x-remaining", "2" );

            //setup mock
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>() )
                .ReturnsAsync( new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent( GetToken() ) } )
                .ReturnsAsync( responseMessage );

            httpClient = new HttpClient( mockHttpMessageHandler.Object );
            return (dict, httpClient);
        }
        public static HttpClient GetMockAvailableResources()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            HttpClient httpClient;
            dict.Add( "Authorization", "Bearer 1234" );
            HttpResponseMessage responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent( GetAvailableResourcesData() ),
                RequestMessage = new HttpRequestMessage() { RequestUri = new Uri( "https://integrate.elluciancloud.com/admin/available-resources" ) }
            };

            //setup mock
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>() )
                .ReturnsAsync( new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent( GetToken() ) } )
                .ReturnsAsync( responseMessage );

            httpClient = new HttpClient( mockHttpMessageHandler.Object );
            return httpClient;
        }
        public static HttpClient GetMockAvailableResourcesWithFiltersAndNamedQueriesData()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            HttpClient httpClient;
            dict.Add( "Authorization", "Bearer 1234" );
            HttpResponseMessage personData = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent( GetResourceForPersons() ),
                RequestMessage = new HttpRequestMessage() { RequestUri = new Uri( "https://integrate.elluciancloud.com/api/persons" ) }
            };

            HttpResponseMessage responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent( GetAvailableResourcesWithFiltersAndNamedQueriesData() ),
                RequestMessage = new HttpRequestMessage() { RequestUri = new Uri( "https://integrate.elluciancloud.com/admin/available-resources" ) }
            };

            //setup mock
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>() )
                .ReturnsAsync( new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent( GetToken() ) } )
                .ReturnsAsync( personData )
                .ReturnsAsync( personData )
                .ReturnsAsync( responseMessage );

            httpClient = new HttpClient( mockHttpMessageHandler.Object );
            return httpClient;
        }
        public static HttpClient GetMockAvailableResourcesForFilterAvailableResources()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            HttpClient httpClient;
            dict.Add( "Authorization", "Bearer 1234" );
            HttpResponseMessage personData = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent( GetAvailableResourcesWithFiltersAndNamedQueriesData() ),
                RequestMessage = new HttpRequestMessage() { RequestUri = new Uri( "https://integrate.elluciancloud.com/admin/available-resources" ) }
            };

            //setup mock
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>() )
                .ReturnsAsync( personData );

            httpClient = new HttpClient( mockHttpMessageHandler.Object );
            return httpClient;
        }

        public static HttpClient GetMockAppConfigFilterAvailableResources()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            HttpClient httpClient;
            dict.Add( "Authorization", "Bearer 1234" );
            HttpResponseMessage configData = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent( GetAppConfig() ),
                RequestMessage = new HttpRequestMessage() { RequestUri = new Uri( "https://integrate.elluciancloud.com/admin/available-resources" ) }
            };

            HttpResponseMessage avaiResourceData = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent( GetAvailableResourcesData() ),
                RequestMessage = new HttpRequestMessage() { RequestUri = new Uri( "https://integrate.elluciancloud.com/admin/available-resources" ) }
            };

            //setup mock
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>() )
                .ReturnsAsync( new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent( GetToken() ) } )
                .ReturnsAsync( configData )
                .ReturnsAsync( avaiResourceData );

            httpClient = new HttpClient( mockHttpMessageHandler.Object );
            return httpClient;
        }

        #endregion
    }
}
