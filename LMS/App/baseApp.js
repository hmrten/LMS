﻿(function () {
    var app = angular.module('base', ['ngRoute']);

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
            fd.append('title', data.title);
            fd.append('description', data.description);
            fd.append('file', data.file);
            return $http.post(LMS.rootPath + url, fd, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            });
        }
    }]);
}());