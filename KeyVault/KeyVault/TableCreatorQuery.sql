create table if not exists User
(
    userId   varchar(250) null,
    userName varchar(250) null,
    email    varchar(250) null,
    password varchar(250) null,
    constraint ownerId
        unique (userId)
);

create table if not exists `Group`
(
    groupId  varchar(250) not null
        primary key,
    title    varchar(250) not null,
    owner_id varchar(250) not null,
    constraint owner_id
        foreign key (owner_id) references User (userId)
);

create table if not exists GroupMember
(
    groupMemberId varchar(250) not null
        primary key,
    groupId       varchar(250) not null,
    memberId      varchar(250) not null,
    constraint groupId
        foreign key (groupId) references `Group` (groupId),
    constraint memberId
        foreign key (memberId) references User (userId)
);

create table if not exists Secret
(
    secretId    varchar(250) not null
        primary key,
    title       varchar(250) not null,
    content     varchar(250) not null,
    dateCreated datetime     not null,
    ownerId     varchar(250) not null,
    constraint ownerId
        foreign key (ownerId) references User (userId)
);

create table if not exists GroupSecret
(
    groupSecretId varchar(250) not null
        primary key,
    group_id      varchar(250) not null,
    secretId      varchar(250) not null,
    constraint group_id
        foreign key (group_id) references `Group` (groupId),
    constraint secretId
        foreign key (secretId) references Secret (secretId)
);