(function () {
    var app = angular.module('schedule', ['base']);

    app.config(function ($routeProvider, $locationProvider) {
        $routeProvider
            .when('/', {
                templateUrl: LMS.rootPath + 'App/Teacher/Schedule/Views/scheduleIndexView.html',
                controller: 'indexCtrl'
            })
            .when('/Show/:id', {
                templateUrl: LMS.rootPath + 'App/Teacher/Schedule/Views/scheduleShowView.html',
                controller: 'showCtrl'
            })
            .otherwise({
                redirectTo: '/'
            });
    });

    app.factory('Data', function () {
        return {
            groupName: ''
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

    app.directive('schedule', function ($parse, $animate) {
        return {
            restrict: 'A',
            scope: {
                month: '=schedule',
                first: '=',
                limit: '=',
                click: '&',
                refresh: '&'
            },
            link: function (scope, el, attr) {
                function inMonth(d) { return d > 0 && d <= scope.limit };
                function calcDay(i) { return i - scope.first + 1 };

                function generate(m) {
                    for (var y = 0; y < 6; ++y) {
                        var tr = angular.element('<tr></tr>');
                        var w = y * 7;
                        for (var x = 0; x < 7; ++x) {
                            var day = calcDay(w + x);
                            var text = inMonth(day) ? day : '-';
                            var td = angular.element('<td class="col-md-1">' + text + '</td>');

                            var onClick = function (d) {
                                var fn = scope.click();
                                return function () {
                                    fn(d);
                                };
                            };

                            td.on('click', onClick(day));
                            tr.append(td);
                        }
                        el.append(tr);
                    }
                    $animate.addClass(el, 'added');
                    scope.$parent.tbody = el;
                    scope.refresh()(el);
                }

                scope.$watch('month', function (val) {
                    el.removeClass('added');
                    el.empty();
                    generate(val);
                });
            }
        };
    });

    app.controller('indexCtrl', function ($scope, $http, Data) {
        $http.get(LMS.rootPath + 'Data/Groups').then(function (resp) {
            $scope.groups = resp.data;
        });

        $scope.setGroupName = function (name) {
            Data.groupName = name;
        };
    });

    app.controller('showCtrl', ['$scope', '$http', '$rootScope', '$routeParams', 'Data', function ($scope, $http, $rootScope, $routeParams, Data) {
        function init() {
            var id = $routeParams['id'];
            $scope.groupId = id;
            $scope.json = [];
            $scope.usedDays = [];
            $scope.curMonth = new Date().getMonth();
            $scope.groupId = id;
            if (Data.groupName == '') {
                $http.get(LMS.rootPath + 'Data/GroupName/' + id).then(function (resp) {
                    $scope.groupName = resp.data;
                });
            } else {
                $scope.groupName = Data.groupName;
            }
            genCal($scope.curMonth);
            getData(id);
        };

        function genCal(month) {
            var now = new Date(2016, month, 1);
            var firstDay = new Date(now.getFullYear(), now.getMonth(), 1);
            $scope.numDays = new Date(now.getFullYear(), now.getMonth() + 1, 0).getDate();
            $scope.firstDay = firstDay.getDay();
            $scope.monthName = monthNames[now.getMonth()];
        };

        function refresh(tbody) {
            var json = $scope.json;
            var trs = tbody.children();
            var sched = [];
            for (var j = 0; j < json.length; ++j) {
                var o = json[j];
                var odate = LMS.parseMSDate(o.date_end);
                if (odate.getMonth() == $scope.curMonth) {
                    var day = odate.getDate();
                    var idx = day + $scope.firstDay - 1;
                    var y = Math.floor(idx / 7);
                    var x = idx % 7;
                    var tr = trs[y];
                    var td = angular.element(tr.children[x]);
                    sched.push({day: day, obj: o });
                    td.addClass('has-events');
                }
            }
            $scope.sched = sched;
        };

        $scope.refresh = refresh;

        function getData(id) {
            $http.get(LMS.rootPath + 'Data/Schedule/' + id).then(function (resp) {
                $scope.json = resp.data;
                refresh($scope.tbody);
            });

            $http.get(LMS.rootPath + 'Data/ScheduleTypes').then(function (resp) {
                $scope.scheduleTypes = resp.data;
            });
            $http.get(LMS.rootPath + 'Data/Subjects').then(function (resp) {
                $scope.subjects = resp.data;
            });
        };

        $scope.addMonth = function (delta) {
            $scope.curMonth += delta;
            genCal($scope.curMonth);
        };

        $scope.select = function (s) {
            $scope.details = null;
            $scope.selectedDay = s;
            $scope.details = $scope.sched;
            $scope.clickedDate = new Date(2016, $scope.curMonth, s);
            $('#details').modal('show');
            $scope.$apply();
        };

        $scope.create = function () {
            var groupId = $scope.groupId;
            this.form.group_id = groupId;
            var t0 = this.form.date_start.split(':');
            var t1 = this.form.date_end.split(':');
            var d0 = new Date(2016, $scope.curMonth, $scope.selectedDay, parseInt(t0[0]), parseInt(t0[1]));
            var d1 = new Date(2016, $scope.curMonth, $scope.selectedDay, parseInt(t1[0]), parseInt(t1[1]));
            this.form.date_start = d0;
            this.form.date_end = d1;

            var json = angular.toJson(this.form);
            $http.post(LMS.rootPath + 'Schedule/Create', json).then(function (resp) {
                $scope.msg = {
                    type: 'success',
                    strong: 'Skapa lyckades!',
                    text: resp.statusText
                };

                $http.get(LMS.rootPath + 'Data/Schedule/' + groupId).then(function (resp) {
                    $scope.json = resp.data;
                    refresh($scope.tbody);
                    $scope.select($scope.selectedDay);
                });
            }, function (resp) {
                $scope.msg = {
                    type: 'danger',
                    strong: 'Skapa misslyckades!',
                    text: resp.status + ': ' + resp.statusText
                };
                $scope.$apply();
            });
        };

        $scope.detailsFilter = function (selectedDay) {
            return function (d) {
                return d.day == selectedDay;
            };
        };

        init();
    }]);

}());