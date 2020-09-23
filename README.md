Причины принятых решений в проекте:
1. Все коллайдеры не являются MeshCollider, потому что по моим данным MeshCollider съедает производительность намного сильнее, чем несколько базовых коллайдеров.
2. Коллайдеры находятся в дочерних объектах, чтобы была возможность изменять разворот коллайдеров.
3. В некоторых местах используются CapsuleColliders для более реалистичного поведения объектов при столкновениях. Например автомат при падении на землю развернется на сторону с большей площадью.
4. В ТЗ написано сделать плавное вынимание объектов из инвентаря, но я сделал выскакивание в сторону камеры и немного вверх. Потому что предметы перетаскиваются по экрану с зажатой мышкой, а вынимание из инвентаря происходит при отпускании мышки на иконке предмета в инвентаре. В итоге могла возникнуть ситуация, где предмет перетаскивается с отпущенной ЛКМ. Чтобы процесс выглядел более корректным, я принял решение вместо плавного вытаскивания сделать выскакивание из инвентаря.



Ссылки на авторов 3д моделей (Все модели распространяются по лицензии CC Attribution):

Трава: автор sam_ogon, ссылка - https://sketchfab.com/3d-models/grass-90-aed86cb3d9b246aa946f45294010aca3

Ружье: автор neutralize, ссылка - https://sketchfab.com/3d-models/riffle-d-2adfaeb89abf49c7976a8eadd01cc098

Топор: автор Kevin Medzorian, ссылка - https://sketchfab.com/3d-models/axe-6a83571675bd4543aee11f3499546ee3

Банка: автор Kevin Medzorian, ссылка - https://sketchfab.com/3d-models/soda-485af512e9844dbdb8543a1652792890

Рюкзак: автор Kevin Medzorian, ссылка - https://sketchfab.com/3d-models/backpack-21d7328419354075bfdb672f2c2370a6
