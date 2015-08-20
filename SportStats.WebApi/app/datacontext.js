/***
 * Service: datacontext 
 *
 * Handles all persistence and creation/deletion of app entities
 * using BreezeJS.
 *
 ***/
(function () {
    'use strict';

    var serviceId = 'datacontext';
    angular.module('app').factory(serviceId,
    ['$q', 'logger', 'entityManagerFactory', datacontext]);

    function datacontext($q, logger, emFactory) {
        logger = logger.forSource(serviceId);
        var logError = logger.logError;
        var logSuccess = logger.logSuccess;
        var logWarning = logger.logWarning;

        var manager = emFactory.newManager();

        var service = {
            getCountries: getCountries
        };

        return service;

        /*** implementation ***/

        function getCountries() {
            var count;
            //Todo: when no forceRefresh, consider getting from cache rather than remotely
            return breeze.EntityQuery.from('Countries')
                //.orderBy('created desc, title')
                //.expand("todoItems")
                .using(manager).execute()
                .then(success).catch(failed);

            function success(response) {
                count = response.results.length;
                logSuccess('Got ' + count + ' countries', response, true);
                return response.results;
            }
            function failed(error) {
                var message = error.message || "countries query failed";
                logError(message, error, true);
            }
        }
    }
})();
