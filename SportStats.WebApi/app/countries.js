/***
 * Controller/ViewModel: countries 
 *
 * Support a view of all CountriesList
 *
 * Handles fetch and save of these lists
 *
 ***/
(function () {
    'use strict';

    var controllerId = 'countries';
    angular.module('app').controller(controllerId,
    ['datacontext', 'logger', countriesController]);

    function countriesController(datacontext, logger) {
        logger = logger.forSource(controllerId);

        var vm = this;
        vm.countriesList = [];

        initialize();

        function initialize() {
            getCountriesList();
        }

        function getCountriesList() {
            return datacontext.getTodoLists().then(function (data) {
                return vm.countriesList = data;
            });
        }
    }
})();