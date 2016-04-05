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
}());