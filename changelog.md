# Ellucian Ethos Integration C# SDK

## Change Log

Date | Version | Description
---- | ------- | -----------
02-10-2021 | 0.2.0 | Added the `Ellucian.Ethos.Integration.Notification` namespace for supporting polling for ChangeNotifications. Added the `Ellucian.Ethos.Integration.Service` namespace supporting the `EthosChangeNotificationService`.
02-23-2021 | 0.3.0 | Updated criteria and named query filter capability to better handle the various combinations of JSON filter syntax and structure.  The `NamedSimpledCritiera`, `SimpleCriteria` and `CriteriaSet` classes were removed and replaced with `CriteriaFilter` and `NamedQueryFilter` classes. Existing `FilterMap` class remain unchanged. `FilterUtility` class was added to handle common methods and functionality used by `CriteriaFilter` and `NamedQueryFilter`. `CriteriaExtensions` and `NamedQueryExtensions` are internal classes which extends `CriteriaFilter` and `NamedQueryFilter` classes by adding extension methods and allows for fluent syntax. Since this version of the SDK is not yet GA, no deprecation of the removed class was given.
03-02-2021 | 0.4.0 | If Ethos web api max page size is smaller than requested page size, then we need to make sure that we adjust the PageSize in pager and pages are returned with correct x-total-count which reflect what api allows in x-max-page-size for each page returned and no rows are omitted. (With Colleague API's, the max page size is either 100 or 200).
03-30-2021 | 0.4.0 | Fixed a bug where paging for num pages or num rows using the EthosFilterQueryClient without a criteria filter, named query, or filter map resulted in null response.
04-15-2021 | 0.4.0 | Added named query support for additional filter syntax structures.
09-29-2021 | 0.5.0 | Created EthosExample class to have central entrance point with Main method. The Main method calls other example class Run method by providing api key and in some cases record guid.
JUN 2022 | 1.0.0 | Added support in the EthosFilterQueryClient for QAPI POST requests.
