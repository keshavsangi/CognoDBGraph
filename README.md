# 🌐 CognoDB Developer Graph

An ASP.NET Core MVC web application backed by *CognoDB Cloud* that models IT developers, technical skills, and relationships to discover direct skills (1-hop) and 2-hop teammate recommendations based on shared skill sets.

---

## 🌟 Why a Graph Database?
In traditional relational databases (SQL), finding developers who share common skills requires complex multi-table JOIN operations across Developers, DeveloperSkills, and Skills tables. As datasets scale, join complexity increases significantly.

By using *CognoDB Cloud* with Cypher pattern matching, relationships are first-class citizens. We query patterns directly:
```text
(Developer)-[:HAS_SKILL]->(Skill)<-[:HAS_SKILL]-(Developer)

## 🟢 Video Demo Link
* [Watch Demo Video](https://www.loom.com/share/a5448aac62d94ec681627a37c2ca1948)
*

