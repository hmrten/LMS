(function () {
    var app = angular.module('base', ['ngRoute', 'ngAnimate']);

    app.directive('fileModel', ['$parse', function ($parse) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                var model = $parse(attrs.fileModel);
                var setter = model.assign;
                element.bind('change', function () {
                    scope.$apply(function () { setter(scope, element[0].files[0]); });
                });
            }
        };
    }]);

    app.service('fileUpload', ['$http', function ($http) {
        this.uploadFile = function (data, url) {
            var fd = new FormData();
            fd.append('model', data.model);
            fd.append('file', data.file);
            return $http.post(LMS.rootPath + url, fd, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            });
        }
    }]);

    app.service('dataService', ['$http', function ($http) {
        this.get = function (url, onSuccess, onError) {

            $http.get(LMS.rootPath + url).then(function (resp) {
                onSuccess(resp.data);
            }, function (resp) {
                onError(resp);
            });
        };
        this.post = function (url, data, onSuccess, onError) {

            $http.post(LMS.rootPath + url, data).then(function (resp) {
                onSuccess(resp);
            }, function (resp) {
                onError(resp);
            });
        };
    }]);

    app.filter('msDate', function () {
        return function (s) {
            return LMS.parseMSDate(s);
        };
    });

    app.filter('dayDiff', function () {
        return function (detail) {
            if (!detail) return 0;
            var date_start = LMS.parseMSDate(detail.date_start);
            var date_end = LMS.parseMSDate(detail.date_end);
            var dt = date_end - date_start;
            return Math.floor(dt / (1000 * 3600 * 24));
        };
    });

    var monthNames = [
    'Januari',
    'Februari',
    'Mars',
    'April',
    'Maj',
    'Juni',
    'Juli',
    'Augusti',
    'September',
    'Oktober',
    'November',
    'December'
    ];

    app.filter('monthName', function () {
        return function (m) {
            return monthNames[m];
        };
    });

    app.filter('schedTypeLabel', function () {
        return function (type) {
            return type == 0 ? 'default' : 'info';
        };
    });

    app.filter('schedTypeName', function () {
        return function (type) {
            return type == 0 ? 'Studier' : 'Möte';
        };
    });
}());