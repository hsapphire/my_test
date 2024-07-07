–1.学生表
Student(SId,Sname,Sage,Ssex)
--SId 学生编号,Sname 学生姓名,Sage 出生年月,Ssex 学生性别

–2.课程表
Course(CId,Cname,TId)
--CId 课程编号,Cname 课程名称,TId 教师编号

–3.教师表
Teacher(TId,Tname)
--TId 教师编号,Tname 教师姓名

--成绩表
--SId 学生编号,CId 课程编号,score 分数

-- 查询" 01 “课程比” 02 "课程成绩高的学生的信息及课程分数

select * from  (
       select t1.SId, class1, class2
       from
           (SELECT SId, score as class1 FROM SC WHERE SC.CId = '01') AS t1, 
           (SELECT SId, score as class2 FROM SC WHERE SC.CId = '02') AS t2
       where t1.SId = t2.SId and t1.class1 > t2.class2
   ) r
   LEFT JOIN Student
   ON Student.SId = r.SId;

-- 1.1 查询同时存在" 01 “课程和” 02 "课程的情况
select * from      
(select * from SC where SC.CId = '01') as t1,     
(select * from SC where SC.CId = '02') as t2
 where t1.SId = t2.SId;


-- 1.2 查询存在" 01 “课程但可能不存在” 02 "课程的情况(不存在时显示为 null )
select * from
    -> (select * from SC where SC.CId = '01') as t1
    -> left join
    -> (select * from SC where SC.CId = '02') as t2
    -> on t1.SId = t2.SId;


