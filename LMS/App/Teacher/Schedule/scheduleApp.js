(function () {
    var app = angular.module('schedule', ['base']);

    app.config(function ($routeProvider, $locationProvider) {
        $routeProvider
            .when('/', {
                templateUrl: LMS.rootPath + 'App/Teacher/Schedule/Views/scheduleIndexView.html',
                controller: 'indexCtrl'
            })
            .otherwise({
                redirectTo: '/'
            });

        //$locationProvider.html5Mode({
        //    enabled: true,
        //    requireBase: false
        //});
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

    app.controller('indexCtrl', ['$scope', '$http', function ($scope, $http) {
        function init() {
            $scope.curMonth = new Date().getMonth();
            genCal($scope.curMonth);
        };

        function genCal(month) {
            var now = new Date(2016, month, 1);
            var firstDay = new Date(now.getFullYear(), now.getMonth(), 1);
            $scope.numDays = new Date(now.getFullYear(), now.getMonth() + 1, 0).getDate();
            $scope.firstDay = firstDay.getDay();
            $scope.monthName = monthNames[now.getMonth()];
        };

        function inMonth(d) { return d > 0 && d <= $scope.numDays };
        function calcDay(i) { return i - $scope.firstDay + 1 };

        $scope.sched = {
            css: function (i) {
                var d = calcDay(i);
                if (!inMonth(d))
                    return '';
                if (d == 10)
                    return 'foo';
            },
            html: function (i) {
                var d = calcDay(i);
                if (!inMonth(d))
                    return '-';
                return d;
            },
            click: function (i) {
                var d = calcDay(i);
                if (inMonth(d)) {
                    alert('you clicked on day: ' + d);
                }
            }
        };

        $scope.addMonth = function (delta) {
            $scope.curMonth += delta;
            genCal($scope.curMonth);
        };

        init();
    }]);
}());