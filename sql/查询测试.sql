
/*join 建表语句*/
drop database if exists test;
create database test;
use test;
 
/* 左表t1*/
drop table if exists t1;
create table t1 (id int not null,name varchar(20));
insert into t1 values (1,'t1a');
insert into t1 values (2,'t1b');
insert into t1 values (3,'t1c');
insert into t1 values (4,'t1d');
insert into t1 values (5,'t1f');
 
/* 右表 t2*/
drop table if exists t2;
create table t2 (id int not null,name varchar(20));
insert into t2 values (2,'t2b');
insert into t2 values (3,'t2c');
insert into t2 values (4,'t2d');
insert into t2 values (5,'t2f');
insert into t2 values (6,'t2a');

/*两表关联，把左表的列和右表的列通过笛卡尔积的形式表达出来。 笛卡尔积  左表*右表每一列*/ 
mysql> select * from t1 join t2;

/*mysql中没有full join。我们可以使用union来达到目的。*/ 
SELECT * FROM t1 LEFT JOIN t2 ON t1.id = t2.id
UNION 
SELECT * FROM t1 RIGHT JOIN t2 ON t1.id = t2.id;
