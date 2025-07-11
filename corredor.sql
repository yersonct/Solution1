PGDMP                      }         
   AnprVision    16.4    16.4 �    �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            �           1262    74552 
   AnprVision    DATABASE        CREATE DATABASE "AnprVision" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Spanish_Spain.1252';
    DROP DATABASE "AnprVision";
                postgres    false            �            1259    74612    user    TABLE     �   CREATE TABLE public."user" (
    id integer NOT NULL,
    username character varying(50) NOT NULL,
    password character varying(255) NOT NULL,
    id_person integer NOT NULL,
    active boolean
);
    DROP TABLE public."user";
       public         heap    postgres    false            �            1259    74611    User_id_user_seq    SEQUENCE     �   CREATE SEQUENCE public."User_id_user_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 )   DROP SEQUENCE public."User_id_user_seq";
       public          postgres    false    228            �           0    0    User_id_user_seq    SEQUENCE OWNED BY     D   ALTER SEQUENCE public."User_id_user_seq" OWNED BY public."user".id;
          public          postgres    false    227            �            1259    74640 	   blacklist    TABLE     �   CREATE TABLE public.blacklist (
    id integer NOT NULL,
    reason character varying(255) NOT NULL,
    restrictiondate date NOT NULL,
    id_client integer NOT NULL,
    active boolean
);
    DROP TABLE public.blacklist;
       public         heap    postgres    false            �            1259    74639    blacklist_id_blacklist_seq    SEQUENCE     �   CREATE SEQUENCE public.blacklist_id_blacklist_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 1   DROP SEQUENCE public.blacklist_id_blacklist_seq;
       public          postgres    false    232            �           0    0    blacklist_id_blacklist_seq    SEQUENCE OWNED BY     O   ALTER SEQUENCE public.blacklist_id_blacklist_seq OWNED BY public.blacklist.id;
          public          postgres    false    231            �            1259    74762    camara    TABLE     -  CREATE TABLE public.camara (
    id integer NOT NULL,
    nightvisioninfrared boolean NOT NULL,
    highresolution boolean NOT NULL,
    infraredlighting boolean NOT NULL,
    name character varying(50) NOT NULL,
    optimizedangleofvision boolean,
    highshutterspeed boolean,
    active boolean
);
    DROP TABLE public.camara;
       public         heap    postgres    false            �            1259    74761    camaras_id_camaras_seq    SEQUENCE     �   CREATE SEQUENCE public.camaras_id_camaras_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 -   DROP SEQUENCE public.camaras_id_camaras_seq;
       public          postgres    false    250            �           0    0    camaras_id_camaras_seq    SEQUENCE OWNED BY     H   ALTER SEQUENCE public.camaras_id_camaras_seq OWNED BY public.camara.id;
          public          postgres    false    249            �            1259    74626    client    TABLE     �   CREATE TABLE public.client (
    id integer NOT NULL,
    id_user integer,
    name character varying(50) NOT NULL,
    active boolean
);
    DROP TABLE public.client;
       public         heap    postgres    false            �            1259    74625    client_id_client_seq    SEQUENCE     �   CREATE SEQUENCE public.client_id_client_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 +   DROP SEQUENCE public.client_id_client_seq;
       public          postgres    false    230            �           0    0    client_id_client_seq    SEQUENCE OWNED BY     F   ALTER SEQUENCE public.client_id_client_seq OWNED BY public.client.id;
          public          postgres    false    229            �            1259    74568 
   formmodule    TABLE     �   CREATE TABLE public.formmodule (
    id integer NOT NULL,
    id_module integer NOT NULL,
    id_forms integer NOT NULL,
    active boolean
);
    DROP TABLE public.formmodule;
       public         heap    postgres    false            �            1259    74567    formmodule_id_seq    SEQUENCE     �   CREATE SEQUENCE public.formmodule_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public.formmodule_id_seq;
       public          postgres    false    220            �           0    0    formmodule_id_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public.formmodule_id_seq OWNED BY public.formmodule.id;
          public          postgres    false    219            �            1259    74680    formrolpermission    TABLE     �   CREATE TABLE public.formrolpermission (
    id integer NOT NULL,
    id_forms integer NOT NULL,
    id_rol integer NOT NULL,
    id_permission integer NOT NULL,
    active boolean
);
 %   DROP TABLE public.formrolpermission;
       public         heap    postgres    false            �            1259    74679 *   formrolpermission_id_formrolpermission_seq    SEQUENCE     �   CREATE SEQUENCE public.formrolpermission_id_formrolpermission_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 A   DROP SEQUENCE public.formrolpermission_id_formrolpermission_seq;
       public          postgres    false    238            �           0    0 *   formrolpermission_id_formrolpermission_seq    SEQUENCE OWNED BY     g   ALTER SEQUENCE public.formrolpermission_id_formrolpermission_seq OWNED BY public.formrolpermission.id;
          public          postgres    false    237            �            1259    74561    forms    TABLE     �   CREATE TABLE public.forms (
    id integer NOT NULL,
    name character varying(100) NOT NULL,
    url character varying(255),
    active boolean
);
    DROP TABLE public.forms;
       public         heap    postgres    false            �            1259    74560    forms_id_forms_seq    SEQUENCE     �   CREATE SEQUENCE public.forms_id_forms_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 )   DROP SEQUENCE public.forms_id_forms_seq;
       public          postgres    false    218            �           0    0    forms_id_forms_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public.forms_id_forms_seq OWNED BY public.forms.id;
          public          postgres    false    217            �            1259    74781    invoice    TABLE       CREATE TABLE public.invoice (
    id integer NOT NULL,
    totalamount numeric(10,2) NOT NULL,
    paymentstatus character varying(50) NOT NULL,
    paymentdate timestamp without time zone NOT NULL,
    id_vehiclehistory integer NOT NULL,
    active boolean
);
    DROP TABLE public.invoice;
       public         heap    postgres    false            �            1259    74780    invoice_id_invoice_seq    SEQUENCE     �   CREATE SEQUENCE public.invoice_id_invoice_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 -   DROP SEQUENCE public.invoice_id_invoice_seq;
       public          postgres    false    254            �           0    0    invoice_id_invoice_seq    SEQUENCE OWNED BY     I   ALTER SEQUENCE public.invoice_id_invoice_seq OWNED BY public.invoice.id;
          public          postgres    false    253            �            1259    74714    memberships    TABLE     �   CREATE TABLE public.memberships (
    id integer NOT NULL,
    membershiptype character varying(50) NOT NULL,
    startdate date NOT NULL,
    enddate date NOT NULL,
    active boolean
);
    DROP TABLE public.memberships;
       public         heap    postgres    false            �            1259    74713    memberships_id_memberships_seq    SEQUENCE     �   CREATE SEQUENCE public.memberships_id_memberships_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 5   DROP SEQUENCE public.memberships_id_memberships_seq;
       public          postgres    false    242            �           0    0    memberships_id_memberships_seq    SEQUENCE OWNED BY     U   ALTER SEQUENCE public.memberships_id_memberships_seq OWNED BY public.memberships.id;
          public          postgres    false    241            �            1259    74721    membershipsvehicle    TABLE     �   CREATE TABLE public.membershipsvehicle (
    id integer NOT NULL,
    id_memberships integer NOT NULL,
    id_vehicle integer NOT NULL,
    active boolean
);
 &   DROP TABLE public.membershipsvehicle;
       public         heap    postgres    false            �            1259    74720 ,   membershipsvehicle_id_membershipsvehicle_seq    SEQUENCE     �   CREATE SEQUENCE public.membershipsvehicle_id_membershipsvehicle_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 C   DROP SEQUENCE public.membershipsvehicle_id_membershipsvehicle_seq;
       public          postgres    false    244            �           0    0 ,   membershipsvehicle_id_membershipsvehicle_seq    SEQUENCE OWNED BY     j   ALTER SEQUENCE public.membershipsvehicle_id_membershipsvehicle_seq OWNED BY public.membershipsvehicle.id;
          public          postgres    false    243            �            1259    74554    module    TABLE     v   CREATE TABLE public.module (
    id integer NOT NULL,
    name character varying(100) NOT NULL,
    active boolean
);
    DROP TABLE public.module;
       public         heap    postgres    false            �            1259    74553    module_id_module_seq    SEQUENCE     �   CREATE SEQUENCE public.module_id_module_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 +   DROP SEQUENCE public.module_id_module_seq;
       public          postgres    false    216            �           0    0    module_id_module_seq    SEQUENCE OWNED BY     F   ALTER SEQUENCE public.module_id_module_seq OWNED BY public.module.id;
          public          postgres    false    215            �            1259    74769    parking    TABLE     �   CREATE TABLE public.parking (
    id integer NOT NULL,
    name character varying(50) NOT NULL,
    location character varying(50) NOT NULL,
    id_camara integer NOT NULL,
    hability character varying NOT NULL,
    active boolean
);
    DROP TABLE public.parking;
       public         heap    postgres    false            �            1259    74768    parking_id_parking_seq    SEQUENCE     �   CREATE SEQUENCE public.parking_id_parking_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 -   DROP SEQUENCE public.parking_id_parking_seq;
       public          postgres    false    252            �           0    0    parking_id_parking_seq    SEQUENCE OWNED BY     I   ALTER SEQUENCE public.parking_id_parking_seq OWNED BY public.parking.id;
          public          postgres    false    251            �            1259    74594 
   permission    TABLE     z   CREATE TABLE public.permission (
    id integer NOT NULL,
    name character varying(255) NOT NULL,
    active boolean
);
    DROP TABLE public.permission;
       public         heap    postgres    false            �            1259    74593    permission_id_permission_seq    SEQUENCE     �   CREATE SEQUENCE public.permission_id_permission_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 3   DROP SEQUENCE public.permission_id_permission_seq;
       public          postgres    false    224            �           0    0    permission_id_permission_seq    SEQUENCE OWNED BY     R   ALTER SEQUENCE public.permission_id_permission_seq OWNED BY public.permission.id;
          public          postgres    false    223            �            1259    74601    person    TABLE       CREATE TABLE public.person (
    id integer NOT NULL,
    name character varying(50) NOT NULL,
    document character varying(20) NOT NULL,
    phone character varying(20),
    lastname character varying(50) NOT NULL,
    email character varying(100),
    active boolean NOT NULL
);
    DROP TABLE public.person;
       public         heap    postgres    false            �            1259    74600    person_id_person_seq    SEQUENCE     �   CREATE SEQUENCE public.person_id_person_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 +   DROP SEQUENCE public.person_id_person_seq;
       public          postgres    false    226            �           0    0    person_id_person_seq    SEQUENCE OWNED BY     F   ALTER SEQUENCE public.person_id_person_seq OWNED BY public.person.id;
          public          postgres    false    225                       1259    74800    rates    TABLE     �   CREATE TABLE public.rates (
    id integer NOT NULL,
    amount numeric(10,2) NOT NULL,
    startduration interval NOT NULL,
    id_typerates integer NOT NULL,
    endduration interval NOT NULL,
    active boolean
);
    DROP TABLE public.rates;
       public         heap    postgres    false                       1259    74799    rates_id_rates_seq    SEQUENCE     �   CREATE SEQUENCE public.rates_id_rates_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 )   DROP SEQUENCE public.rates_id_rates_seq;
       public          postgres    false    258            �           0    0    rates_id_rates_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public.rates_id_rates_seq OWNED BY public.rates.id;
          public          postgres    false    257            �            1259    74668    registeredvehicle    TABLE     �   CREATE TABLE public.registeredvehicle (
    id integer NOT NULL,
    entrydatetime time without time zone,
    exitdatetime time without time zone,
    id_vehicle integer NOT NULL,
    active boolean
);
 %   DROP TABLE public.registeredvehicle;
       public         heap    postgres    false            �            1259    74667 *   registeredvehicle_id_registeredvehicle_seq    SEQUENCE     �   CREATE SEQUENCE public.registeredvehicle_id_registeredvehicle_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 A   DROP SEQUENCE public.registeredvehicle_id_registeredvehicle_seq;
       public          postgres    false    236            �           0    0 *   registeredvehicle_id_registeredvehicle_seq    SEQUENCE OWNED BY     g   ALTER SEQUENCE public.registeredvehicle_id_registeredvehicle_seq OWNED BY public.registeredvehicle.id;
          public          postgres    false    235            �            1259    74585    rol    TABLE     �   CREATE TABLE public.rol (
    id integer NOT NULL,
    name character varying(30) NOT NULL,
    description text,
    active boolean
);
    DROP TABLE public.rol;
       public         heap    postgres    false            �            1259    74584    role_id_role_seq    SEQUENCE     �   CREATE SEQUENCE public.role_id_role_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 '   DROP SEQUENCE public.role_id_role_seq;
       public          postgres    false    222            �           0    0    role_id_role_seq    SEQUENCE OWNED BY     ?   ALTER SEQUENCE public.role_id_role_seq OWNED BY public.rol.id;
          public          postgres    false    221            �            1259    74702    roluser    TABLE     �   CREATE TABLE public.roluser (
    id integer NOT NULL,
    id_rol integer NOT NULL,
    id_user integer NOT NULL,
    active boolean
);
    DROP TABLE public.roluser;
       public         heap    postgres    false            �            1259    74701    roluser_id_roluser_seq    SEQUENCE     �   CREATE SEQUENCE public.roluser_id_roluser_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 -   DROP SEQUENCE public.roluser_id_roluser_seq;
       public          postgres    false    240            �           0    0    roluser_id_roluser_seq    SEQUENCE OWNED BY     I   ALTER SEQUENCE public.roluser_id_roluser_seq OWNED BY public.roluser.id;
          public          postgres    false    239                        1259    74793 	   typerates    TABLE     �   CREATE TABLE public.typerates (
    id integer NOT NULL,
    name character varying(50) NOT NULL,
    price numeric(10,2) NOT NULL,
    active boolean
);
    DROP TABLE public.typerates;
       public         heap    postgres    false            �            1259    74792    typerates_id_typerates_seq    SEQUENCE     �   CREATE SEQUENCE public.typerates_id_typerates_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 1   DROP SEQUENCE public.typerates_id_typerates_seq;
       public          postgres    false    256            �           0    0    typerates_id_typerates_seq    SEQUENCE OWNED BY     O   ALTER SEQUENCE public.typerates_id_typerates_seq OWNED BY public.typerates.id;
          public          postgres    false    255            �            1259    74738    typevehicles    TABLE     {   CREATE TABLE public.typevehicles (
    id integer NOT NULL,
    name character varying(50) NOT NULL,
    active boolean
);
     DROP TABLE public.typevehicles;
       public         heap    postgres    false            �            1259    74737    typevehicle_id_typevehicle_seq    SEQUENCE     �   CREATE SEQUENCE public.typevehicle_id_typevehicle_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 5   DROP SEQUENCE public.typevehicle_id_typevehicle_seq;
       public          postgres    false    246            �           0    0    typevehicle_id_typevehicle_seq    SEQUENCE OWNED BY     V   ALTER SEQUENCE public.typevehicle_id_typevehicle_seq OWNED BY public.typevehicles.id;
          public          postgres    false    245            �            1259    74654    vehicle    TABLE     �   CREATE TABLE public.vehicle (
    id integer NOT NULL,
    plate character varying(50) NOT NULL,
    color character varying(100),
    id_client integer NOT NULL,
    active boolean
);
    DROP TABLE public.vehicle;
       public         heap    postgres    false            �            1259    74653    vehicle_id_vehicle_seq    SEQUENCE     �   CREATE SEQUENCE public.vehicle_id_vehicle_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 -   DROP SEQUENCE public.vehicle_id_vehicle_seq;
       public          postgres    false    234            �           0    0    vehicle_id_vehicle_seq    SEQUENCE OWNED BY     I   ALTER SEQUENCE public.vehicle_id_vehicle_seq OWNED BY public.vehicle.id;
          public          postgres    false    233            �            1259    74745    vehiclehistory    TABLE     �   CREATE TABLE public.vehiclehistory (
    id integer NOT NULL,
    totaltime time without time zone NOT NULL,
    id_typevehicle integer NOT NULL,
    id_registeredvehicle integer NOT NULL,
    id_invoice integer,
    active boolean
);
 "   DROP TABLE public.vehiclehistory;
       public         heap    postgres    false            �            1259    74744 $   vehiclehistory_id_vehiclehistory_seq    SEQUENCE     �   CREATE SEQUENCE public.vehiclehistory_id_vehiclehistory_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ;   DROP SEQUENCE public.vehiclehistory_id_vehiclehistory_seq;
       public          postgres    false    248            �           0    0 $   vehiclehistory_id_vehiclehistory_seq    SEQUENCE OWNED BY     ^   ALTER SEQUENCE public.vehiclehistory_id_vehiclehistory_seq OWNED BY public.vehiclehistory.id;
          public          postgres    false    247                       1259    74812    vehiclehistoryparkingrates    TABLE       CREATE TABLE public.vehiclehistoryparkingrates (
    id integer NOT NULL,
    hourused integer NOT NULL,
    id_rates integer NOT NULL,
    id_vehiclehistory integer NOT NULL,
    id_parking integer NOT NULL,
    subtotal numeric(10,2) NOT NULL,
    active boolean
);
 .   DROP TABLE public.vehiclehistoryparkingrates;
       public         heap    postgres    false                       1259    74811 <   vehiclehistoryparkingrates_id_vehiclehistoryparkingrates_seq    SEQUENCE     �   CREATE SEQUENCE public.vehiclehistoryparkingrates_id_vehiclehistoryparkingrates_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 S   DROP SEQUENCE public.vehiclehistoryparkingrates_id_vehiclehistoryparkingrates_seq;
       public          postgres    false    260            �           0    0 <   vehiclehistoryparkingrates_id_vehiclehistoryparkingrates_seq    SEQUENCE OWNED BY     �   ALTER SEQUENCE public.vehiclehistoryparkingrates_id_vehiclehistoryparkingrates_seq OWNED BY public.vehiclehistoryparkingrates.id;
          public          postgres    false    259            �           2604    74643    blacklist id    DEFAULT     v   ALTER TABLE ONLY public.blacklist ALTER COLUMN id SET DEFAULT nextval('public.blacklist_id_blacklist_seq'::regclass);
 ;   ALTER TABLE public.blacklist ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    232    231    232            �           2604    74765 	   camara id    DEFAULT     o   ALTER TABLE ONLY public.camara ALTER COLUMN id SET DEFAULT nextval('public.camaras_id_camaras_seq'::regclass);
 8   ALTER TABLE public.camara ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    250    249    250            �           2604    74629 	   client id    DEFAULT     m   ALTER TABLE ONLY public.client ALTER COLUMN id SET DEFAULT nextval('public.client_id_client_seq'::regclass);
 8   ALTER TABLE public.client ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    229    230    230            �           2604    74571    formmodule id    DEFAULT     n   ALTER TABLE ONLY public.formmodule ALTER COLUMN id SET DEFAULT nextval('public.formmodule_id_seq'::regclass);
 <   ALTER TABLE public.formmodule ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    220    219    220            �           2604    74683    formrolpermission id    DEFAULT     �   ALTER TABLE ONLY public.formrolpermission ALTER COLUMN id SET DEFAULT nextval('public.formrolpermission_id_formrolpermission_seq'::regclass);
 C   ALTER TABLE public.formrolpermission ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    237    238    238            �           2604    74564    forms id    DEFAULT     j   ALTER TABLE ONLY public.forms ALTER COLUMN id SET DEFAULT nextval('public.forms_id_forms_seq'::regclass);
 7   ALTER TABLE public.forms ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    217    218    218            �           2604    74784 
   invoice id    DEFAULT     p   ALTER TABLE ONLY public.invoice ALTER COLUMN id SET DEFAULT nextval('public.invoice_id_invoice_seq'::regclass);
 9   ALTER TABLE public.invoice ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    253    254    254            �           2604    74717    memberships id    DEFAULT     |   ALTER TABLE ONLY public.memberships ALTER COLUMN id SET DEFAULT nextval('public.memberships_id_memberships_seq'::regclass);
 =   ALTER TABLE public.memberships ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    242    241    242            �           2604    74724    membershipsvehicle id    DEFAULT     �   ALTER TABLE ONLY public.membershipsvehicle ALTER COLUMN id SET DEFAULT nextval('public.membershipsvehicle_id_membershipsvehicle_seq'::regclass);
 D   ALTER TABLE public.membershipsvehicle ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    244    243    244            �           2604    74557 	   module id    DEFAULT     m   ALTER TABLE ONLY public.module ALTER COLUMN id SET DEFAULT nextval('public.module_id_module_seq'::regclass);
 8   ALTER TABLE public.module ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    215    216    216            �           2604    74772 
   parking id    DEFAULT     p   ALTER TABLE ONLY public.parking ALTER COLUMN id SET DEFAULT nextval('public.parking_id_parking_seq'::regclass);
 9   ALTER TABLE public.parking ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    252    251    252            �           2604    74597    permission id    DEFAULT     y   ALTER TABLE ONLY public.permission ALTER COLUMN id SET DEFAULT nextval('public.permission_id_permission_seq'::regclass);
 <   ALTER TABLE public.permission ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    224    223    224            �           2604    74604 	   person id    DEFAULT     m   ALTER TABLE ONLY public.person ALTER COLUMN id SET DEFAULT nextval('public.person_id_person_seq'::regclass);
 8   ALTER TABLE public.person ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    225    226    226            �           2604    74803    rates id    DEFAULT     j   ALTER TABLE ONLY public.rates ALTER COLUMN id SET DEFAULT nextval('public.rates_id_rates_seq'::regclass);
 7   ALTER TABLE public.rates ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    257    258    258            �           2604    74671    registeredvehicle id    DEFAULT     �   ALTER TABLE ONLY public.registeredvehicle ALTER COLUMN id SET DEFAULT nextval('public.registeredvehicle_id_registeredvehicle_seq'::regclass);
 C   ALTER TABLE public.registeredvehicle ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    236    235    236            �           2604    74588    rol id    DEFAULT     f   ALTER TABLE ONLY public.rol ALTER COLUMN id SET DEFAULT nextval('public.role_id_role_seq'::regclass);
 5   ALTER TABLE public.rol ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    222    221    222            �           2604    74705 
   roluser id    DEFAULT     p   ALTER TABLE ONLY public.roluser ALTER COLUMN id SET DEFAULT nextval('public.roluser_id_roluser_seq'::regclass);
 9   ALTER TABLE public.roluser ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    240    239    240            �           2604    74796    typerates id    DEFAULT     v   ALTER TABLE ONLY public.typerates ALTER COLUMN id SET DEFAULT nextval('public.typerates_id_typerates_seq'::regclass);
 ;   ALTER TABLE public.typerates ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    255    256    256            �           2604    74741    typevehicles id    DEFAULT     }   ALTER TABLE ONLY public.typevehicles ALTER COLUMN id SET DEFAULT nextval('public.typevehicle_id_typevehicle_seq'::regclass);
 >   ALTER TABLE public.typevehicles ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    245    246    246            �           2604    74615    user id    DEFAULT     k   ALTER TABLE ONLY public."user" ALTER COLUMN id SET DEFAULT nextval('public."User_id_user_seq"'::regclass);
 8   ALTER TABLE public."user" ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    228    227    228            �           2604    74657 
   vehicle id    DEFAULT     p   ALTER TABLE ONLY public.vehicle ALTER COLUMN id SET DEFAULT nextval('public.vehicle_id_vehicle_seq'::regclass);
 9   ALTER TABLE public.vehicle ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    233    234    234            �           2604    74748    vehiclehistory id    DEFAULT     �   ALTER TABLE ONLY public.vehiclehistory ALTER COLUMN id SET DEFAULT nextval('public.vehiclehistory_id_vehiclehistory_seq'::regclass);
 @   ALTER TABLE public.vehiclehistory ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    248    247    248            �           2604    74815    vehiclehistoryparkingrates id    DEFAULT     �   ALTER TABLE ONLY public.vehiclehistoryparkingrates ALTER COLUMN id SET DEFAULT nextval('public.vehiclehistoryparkingrates_id_vehiclehistoryparkingrates_seq'::regclass);
 L   ALTER TABLE public.vehiclehistoryparkingrates ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    260    259    260            �          0    74640 	   blacklist 
   TABLE DATA           S   COPY public.blacklist (id, reason, restrictiondate, id_client, active) FROM stdin;
    public          postgres    false    232   n�       �          0    74762    camara 
   TABLE DATA           �   COPY public.camara (id, nightvisioninfrared, highresolution, infraredlighting, name, optimizedangleofvision, highshutterspeed, active) FROM stdin;
    public          postgres    false    250   ��       �          0    74626    client 
   TABLE DATA           ;   COPY public.client (id, id_user, name, active) FROM stdin;
    public          postgres    false    230   ��       �          0    74568 
   formmodule 
   TABLE DATA           E   COPY public.formmodule (id, id_module, id_forms, active) FROM stdin;
    public          postgres    false    220   �       �          0    74680    formrolpermission 
   TABLE DATA           X   COPY public.formrolpermission (id, id_forms, id_rol, id_permission, active) FROM stdin;
    public          postgres    false    238   =�                 0    74561    forms 
   TABLE DATA           6   COPY public.forms (id, name, url, active) FROM stdin;
    public          postgres    false    218   n�       �          0    74781    invoice 
   TABLE DATA           i   COPY public.invoice (id, totalamount, paymentstatus, paymentdate, id_vehiclehistory, active) FROM stdin;
    public          postgres    false    254   ��       �          0    74714    memberships 
   TABLE DATA           U   COPY public.memberships (id, membershiptype, startdate, enddate, active) FROM stdin;
    public          postgres    false    242   ��       �          0    74721    membershipsvehicle 
   TABLE DATA           T   COPY public.membershipsvehicle (id, id_memberships, id_vehicle, active) FROM stdin;
    public          postgres    false    244   +�       }          0    74554    module 
   TABLE DATA           2   COPY public.module (id, name, active) FROM stdin;
    public          postgres    false    216   H�       �          0    74769    parking 
   TABLE DATA           R   COPY public.parking (id, name, location, id_camara, hability, active) FROM stdin;
    public          postgres    false    252   z�       �          0    74594 
   permission 
   TABLE DATA           6   COPY public.permission (id, name, active) FROM stdin;
    public          postgres    false    224   ��       �          0    74601    person 
   TABLE DATA           T   COPY public.person (id, name, document, phone, lastname, email, active) FROM stdin;
    public          postgres    false    226   �       �          0    74800    rates 
   TABLE DATA           ]   COPY public.rates (id, amount, startduration, id_typerates, endduration, active) FROM stdin;
    public          postgres    false    258   ��       �          0    74668    registeredvehicle 
   TABLE DATA           `   COPY public.registeredvehicle (id, entrydatetime, exitdatetime, id_vehicle, active) FROM stdin;
    public          postgres    false    236   "�       �          0    74585    rol 
   TABLE DATA           <   COPY public.rol (id, name, description, active) FROM stdin;
    public          postgres    false    222   ?�       �          0    74702    roluser 
   TABLE DATA           >   COPY public.roluser (id, id_rol, id_user, active) FROM stdin;
    public          postgres    false    240   ��       �          0    74793 	   typerates 
   TABLE DATA           <   COPY public.typerates (id, name, price, active) FROM stdin;
    public          postgres    false    256   ��       �          0    74738    typevehicles 
   TABLE DATA           8   COPY public.typevehicles (id, name, active) FROM stdin;
    public          postgres    false    246   M�       �          0    74612    user 
   TABLE DATA           K   COPY public."user" (id, username, password, id_person, active) FROM stdin;
    public          postgres    false    228   v�       �          0    74654    vehicle 
   TABLE DATA           F   COPY public.vehicle (id, plate, color, id_client, active) FROM stdin;
    public          postgres    false    234   ��       �          0    74745    vehiclehistory 
   TABLE DATA           q   COPY public.vehiclehistory (id, totaltime, id_typevehicle, id_registeredvehicle, id_invoice, active) FROM stdin;
    public          postgres    false    248   ��       �          0    74812    vehiclehistoryparkingrates 
   TABLE DATA           }   COPY public.vehiclehistoryparkingrates (id, hourused, id_rates, id_vehiclehistory, id_parking, subtotal, active) FROM stdin;
    public          postgres    false    260   ��       �           0    0    User_id_user_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('public."User_id_user_seq"', 21, true);
          public          postgres    false    227            �           0    0    blacklist_id_blacklist_seq    SEQUENCE SET     I   SELECT pg_catalog.setval('public.blacklist_id_blacklist_seq', 16, true);
          public          postgres    false    231            �           0    0    camaras_id_camaras_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public.camaras_id_camaras_seq', 8, true);
          public          postgres    false    249            �           0    0    client_id_client_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public.client_id_client_seq', 5, true);
          public          postgres    false    229            �           0    0    formmodule_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.formmodule_id_seq', 5, true);
          public          postgres    false    219            �           0    0 *   formrolpermission_id_formrolpermission_seq    SEQUENCE SET     Y   SELECT pg_catalog.setval('public.formrolpermission_id_formrolpermission_seq', 16, true);
          public          postgres    false    237            �           0    0    forms_id_forms_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('public.forms_id_forms_seq', 8, true);
          public          postgres    false    217            �           0    0    invoice_id_invoice_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public.invoice_id_invoice_seq', 2, true);
          public          postgres    false    253            �           0    0    memberships_id_memberships_seq    SEQUENCE SET     L   SELECT pg_catalog.setval('public.memberships_id_memberships_seq', 4, true);
          public          postgres    false    241            �           0    0 ,   membershipsvehicle_id_membershipsvehicle_seq    SEQUENCE SET     Z   SELECT pg_catalog.setval('public.membershipsvehicle_id_membershipsvehicle_seq', 1, true);
          public          postgres    false    243            �           0    0    module_id_module_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public.module_id_module_seq', 6, true);
          public          postgres    false    215            �           0    0    parking_id_parking_seq    SEQUENCE SET     E   SELECT pg_catalog.setval('public.parking_id_parking_seq', 10, true);
          public          postgres    false    251            �           0    0    permission_id_permission_seq    SEQUENCE SET     J   SELECT pg_catalog.setval('public.permission_id_permission_seq', 9, true);
          public          postgres    false    223            �           0    0    person_id_person_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public.person_id_person_seq', 10, true);
          public          postgres    false    225            �           0    0    rates_id_rates_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('public.rates_id_rates_seq', 4, true);
          public          postgres    false    257            �           0    0 *   registeredvehicle_id_registeredvehicle_seq    SEQUENCE SET     X   SELECT pg_catalog.setval('public.registeredvehicle_id_registeredvehicle_seq', 2, true);
          public          postgres    false    235            �           0    0    role_id_role_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public.role_id_role_seq', 7, true);
          public          postgres    false    221            �           0    0    roluser_id_roluser_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public.roluser_id_roluser_seq', 7, true);
          public          postgres    false    239            �           0    0    typerates_id_typerates_seq    SEQUENCE SET     I   SELECT pg_catalog.setval('public.typerates_id_typerates_seq', 13, true);
          public          postgres    false    255            �           0    0    typevehicle_id_typevehicle_seq    SEQUENCE SET     L   SELECT pg_catalog.setval('public.typevehicle_id_typevehicle_seq', 2, true);
          public          postgres    false    245            �           0    0    vehicle_id_vehicle_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public.vehicle_id_vehicle_seq', 6, true);
          public          postgres    false    233            �           0    0 $   vehiclehistory_id_vehiclehistory_seq    SEQUENCE SET     R   SELECT pg_catalog.setval('public.vehiclehistory_id_vehiclehistory_seq', 2, true);
          public          postgres    false    247            �           0    0 <   vehiclehistoryparkingrates_id_vehiclehistoryparkingrates_seq    SEQUENCE SET     j   SELECT pg_catalog.setval('public.vehiclehistoryparkingrates_id_vehiclehistoryparkingrates_seq', 3, true);
          public          postgres    false    259            �           2606    74617    user User_pkey 
   CONSTRAINT     P   ALTER TABLE ONLY public."user"
    ADD CONSTRAINT "User_pkey" PRIMARY KEY (id);
 <   ALTER TABLE ONLY public."user" DROP CONSTRAINT "User_pkey";
       public            postgres    false    228            �           2606    98997    user User_username_key 
   CONSTRAINT     Y   ALTER TABLE ONLY public."user"
    ADD CONSTRAINT "User_username_key" UNIQUE (username);
 D   ALTER TABLE ONLY public."user" DROP CONSTRAINT "User_username_key";
       public            postgres    false    228            �           2606    74645    blacklist blacklist_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public.blacklist
    ADD CONSTRAINT blacklist_pkey PRIMARY KEY (id);
 B   ALTER TABLE ONLY public.blacklist DROP CONSTRAINT blacklist_pkey;
       public            postgres    false    232            �           2606    74767    camara camaras_pkey 
   CONSTRAINT     Q   ALTER TABLE ONLY public.camara
    ADD CONSTRAINT camaras_pkey PRIMARY KEY (id);
 =   ALTER TABLE ONLY public.camara DROP CONSTRAINT camaras_pkey;
       public            postgres    false    250            �           2606    74633    client client_id_user_key 
   CONSTRAINT     W   ALTER TABLE ONLY public.client
    ADD CONSTRAINT client_id_user_key UNIQUE (id_user);
 C   ALTER TABLE ONLY public.client DROP CONSTRAINT client_id_user_key;
       public            postgres    false    230            �           2606    74631    client client_pkey 
   CONSTRAINT     P   ALTER TABLE ONLY public.client
    ADD CONSTRAINT client_pkey PRIMARY KEY (id);
 <   ALTER TABLE ONLY public.client DROP CONSTRAINT client_pkey;
       public            postgres    false    230            �           2606    74573    formmodule formmodule_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY public.formmodule
    ADD CONSTRAINT formmodule_pkey PRIMARY KEY (id);
 D   ALTER TABLE ONLY public.formmodule DROP CONSTRAINT formmodule_pkey;
       public            postgres    false    220            �           2606    74685 (   formrolpermission formrolpermission_pkey 
   CONSTRAINT     f   ALTER TABLE ONLY public.formrolpermission
    ADD CONSTRAINT formrolpermission_pkey PRIMARY KEY (id);
 R   ALTER TABLE ONLY public.formrolpermission DROP CONSTRAINT formrolpermission_pkey;
       public            postgres    false    238            �           2606    74566    forms forms_pkey 
   CONSTRAINT     N   ALTER TABLE ONLY public.forms
    ADD CONSTRAINT forms_pkey PRIMARY KEY (id);
 :   ALTER TABLE ONLY public.forms DROP CONSTRAINT forms_pkey;
       public            postgres    false    218            �           2606    74786    invoice invoice_pkey 
   CONSTRAINT     R   ALTER TABLE ONLY public.invoice
    ADD CONSTRAINT invoice_pkey PRIMARY KEY (id);
 >   ALTER TABLE ONLY public.invoice DROP CONSTRAINT invoice_pkey;
       public            postgres    false    254            �           2606    74719    memberships memberships_pkey 
   CONSTRAINT     Z   ALTER TABLE ONLY public.memberships
    ADD CONSTRAINT memberships_pkey PRIMARY KEY (id);
 F   ALTER TABLE ONLY public.memberships DROP CONSTRAINT memberships_pkey;
       public            postgres    false    242            �           2606    74726 *   membershipsvehicle membershipsvehicle_pkey 
   CONSTRAINT     h   ALTER TABLE ONLY public.membershipsvehicle
    ADD CONSTRAINT membershipsvehicle_pkey PRIMARY KEY (id);
 T   ALTER TABLE ONLY public.membershipsvehicle DROP CONSTRAINT membershipsvehicle_pkey;
       public            postgres    false    244            �           2606    74559    module module_pkey 
   CONSTRAINT     P   ALTER TABLE ONLY public.module
    ADD CONSTRAINT module_pkey PRIMARY KEY (id);
 <   ALTER TABLE ONLY public.module DROP CONSTRAINT module_pkey;
       public            postgres    false    216            �           2606    74774    parking parking_pkey 
   CONSTRAINT     R   ALTER TABLE ONLY public.parking
    ADD CONSTRAINT parking_pkey PRIMARY KEY (id);
 >   ALTER TABLE ONLY public.parking DROP CONSTRAINT parking_pkey;
       public            postgres    false    252            �           2606    74599    permission permission_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY public.permission
    ADD CONSTRAINT permission_pkey PRIMARY KEY (id);
 D   ALTER TABLE ONLY public.permission DROP CONSTRAINT permission_pkey;
       public            postgres    false    224            �           2606    74608    person person_document_key 
   CONSTRAINT     Y   ALTER TABLE ONLY public.person
    ADD CONSTRAINT person_document_key UNIQUE (document);
 D   ALTER TABLE ONLY public.person DROP CONSTRAINT person_document_key;
       public            postgres    false    226            �           2606    74610    person person_email_key 
   CONSTRAINT     S   ALTER TABLE ONLY public.person
    ADD CONSTRAINT person_email_key UNIQUE (email);
 A   ALTER TABLE ONLY public.person DROP CONSTRAINT person_email_key;
       public            postgres    false    226            �           2606    74606    person person_pkey 
   CONSTRAINT     P   ALTER TABLE ONLY public.person
    ADD CONSTRAINT person_pkey PRIMARY KEY (id);
 <   ALTER TABLE ONLY public.person DROP CONSTRAINT person_pkey;
       public            postgres    false    226            �           2606    74805    rates rates_pkey 
   CONSTRAINT     N   ALTER TABLE ONLY public.rates
    ADD CONSTRAINT rates_pkey PRIMARY KEY (id);
 :   ALTER TABLE ONLY public.rates DROP CONSTRAINT rates_pkey;
       public            postgres    false    258            �           2606    74673 (   registeredvehicle registeredvehicle_pkey 
   CONSTRAINT     f   ALTER TABLE ONLY public.registeredvehicle
    ADD CONSTRAINT registeredvehicle_pkey PRIMARY KEY (id);
 R   ALTER TABLE ONLY public.registeredvehicle DROP CONSTRAINT registeredvehicle_pkey;
       public            postgres    false    236            �           2606    74592    rol role_pkey 
   CONSTRAINT     K   ALTER TABLE ONLY public.rol
    ADD CONSTRAINT role_pkey PRIMARY KEY (id);
 7   ALTER TABLE ONLY public.rol DROP CONSTRAINT role_pkey;
       public            postgres    false    222            �           2606    74707    roluser roluser_pkey 
   CONSTRAINT     R   ALTER TABLE ONLY public.roluser
    ADD CONSTRAINT roluser_pkey PRIMARY KEY (id);
 >   ALTER TABLE ONLY public.roluser DROP CONSTRAINT roluser_pkey;
       public            postgres    false    240            �           2606    74798    typerates typerates_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public.typerates
    ADD CONSTRAINT typerates_pkey PRIMARY KEY (id);
 B   ALTER TABLE ONLY public.typerates DROP CONSTRAINT typerates_pkey;
       public            postgres    false    256            �           2606    74743    typevehicles typevehicle_pkey 
   CONSTRAINT     [   ALTER TABLE ONLY public.typevehicles
    ADD CONSTRAINT typevehicle_pkey PRIMARY KEY (id);
 G   ALTER TABLE ONLY public.typevehicles DROP CONSTRAINT typevehicle_pkey;
       public            postgres    false    246            �           2606    74659    vehicle vehicle_pkey 
   CONSTRAINT     R   ALTER TABLE ONLY public.vehicle
    ADD CONSTRAINT vehicle_pkey PRIMARY KEY (id);
 >   ALTER TABLE ONLY public.vehicle DROP CONSTRAINT vehicle_pkey;
       public            postgres    false    234            �           2606    74661    vehicle vehicle_plate_key 
   CONSTRAINT     U   ALTER TABLE ONLY public.vehicle
    ADD CONSTRAINT vehicle_plate_key UNIQUE (plate);
 C   ALTER TABLE ONLY public.vehicle DROP CONSTRAINT vehicle_plate_key;
       public            postgres    false    234            �           2606    74750 "   vehiclehistory vehiclehistory_pkey 
   CONSTRAINT     `   ALTER TABLE ONLY public.vehiclehistory
    ADD CONSTRAINT vehiclehistory_pkey PRIMARY KEY (id);
 L   ALTER TABLE ONLY public.vehiclehistory DROP CONSTRAINT vehiclehistory_pkey;
       public            postgres    false    248            �           2606    74817 :   vehiclehistoryparkingrates vehiclehistoryparkingrates_pkey 
   CONSTRAINT     x   ALTER TABLE ONLY public.vehiclehistoryparkingrates
    ADD CONSTRAINT vehiclehistoryparkingrates_pkey PRIMARY KEY (id);
 d   ALTER TABLE ONLY public.vehiclehistoryparkingrates DROP CONSTRAINT vehiclehistoryparkingrates_pkey;
       public            postgres    false    260            �           2606    74775    parking fk_camaras    FK CONSTRAINT     �   ALTER TABLE ONLY public.parking
    ADD CONSTRAINT fk_camaras FOREIGN KEY (id_camara) REFERENCES public.camara(id) ON DELETE CASCADE;
 <   ALTER TABLE ONLY public.parking DROP CONSTRAINT fk_camaras;
       public          postgres    false    252    4812    250            �           2606    74648    blacklist fk_client    FK CONSTRAINT     �   ALTER TABLE ONLY public.blacklist
    ADD CONSTRAINT fk_client FOREIGN KEY (id_client) REFERENCES public.client(id) ON DELETE CASCADE;
 =   ALTER TABLE ONLY public.blacklist DROP CONSTRAINT fk_client;
       public          postgres    false    4790    230    232            �           2606    74662    vehicle fk_client    FK CONSTRAINT     �   ALTER TABLE ONLY public.vehicle
    ADD CONSTRAINT fk_client FOREIGN KEY (id_client) REFERENCES public.client(id) ON DELETE CASCADE;
 ;   ALTER TABLE ONLY public.vehicle DROP CONSTRAINT fk_client;
       public          postgres    false    230    4790    234            �           2606    74579    formmodule fk_forms    FK CONSTRAINT     �   ALTER TABLE ONLY public.formmodule
    ADD CONSTRAINT fk_forms FOREIGN KEY (id_forms) REFERENCES public.forms(id) ON DELETE CASCADE;
 =   ALTER TABLE ONLY public.formmodule DROP CONSTRAINT fk_forms;
       public          postgres    false    218    4770    220            �           2606    74691    formrolpermission fk_forms    FK CONSTRAINT     �   ALTER TABLE ONLY public.formrolpermission
    ADD CONSTRAINT fk_forms FOREIGN KEY (id_forms) REFERENCES public.forms(id) ON DELETE CASCADE;
 D   ALTER TABLE ONLY public.formrolpermission DROP CONSTRAINT fk_forms;
       public          postgres    false    238    4770    218            �           2606    74727 !   membershipsvehicle fk_memberships    FK CONSTRAINT     �   ALTER TABLE ONLY public.membershipsvehicle
    ADD CONSTRAINT fk_memberships FOREIGN KEY (id_memberships) REFERENCES public.memberships(id) ON DELETE CASCADE;
 K   ALTER TABLE ONLY public.membershipsvehicle DROP CONSTRAINT fk_memberships;
       public          postgres    false    242    244    4804            �           2606    74574    formmodule fk_module    FK CONSTRAINT     �   ALTER TABLE ONLY public.formmodule
    ADD CONSTRAINT fk_module FOREIGN KEY (id_module) REFERENCES public.module(id) ON DELETE CASCADE;
 >   ALTER TABLE ONLY public.formmodule DROP CONSTRAINT fk_module;
       public          postgres    false    4768    216    220            �           2606    74828 %   vehiclehistoryparkingrates fk_parking    FK CONSTRAINT     �   ALTER TABLE ONLY public.vehiclehistoryparkingrates
    ADD CONSTRAINT fk_parking FOREIGN KEY (id_parking) REFERENCES public.parking(id) ON DELETE CASCADE;
 O   ALTER TABLE ONLY public.vehiclehistoryparkingrates DROP CONSTRAINT fk_parking;
       public          postgres    false    4814    252    260            �           2606    74696    formrolpermission fk_permission    FK CONSTRAINT     �   ALTER TABLE ONLY public.formrolpermission
    ADD CONSTRAINT fk_permission FOREIGN KEY (id_permission) REFERENCES public.permission(id) ON DELETE CASCADE;
 I   ALTER TABLE ONLY public.formrolpermission DROP CONSTRAINT fk_permission;
       public          postgres    false    224    4776    238            �           2606    74620    user fk_person    FK CONSTRAINT     �   ALTER TABLE ONLY public."user"
    ADD CONSTRAINT fk_person FOREIGN KEY (id_person) REFERENCES public.person(id) ON DELETE CASCADE;
 :   ALTER TABLE ONLY public."user" DROP CONSTRAINT fk_person;
       public          postgres    false    228    4782    226            �           2606    74818 #   vehiclehistoryparkingrates fk_rates    FK CONSTRAINT     �   ALTER TABLE ONLY public.vehiclehistoryparkingrates
    ADD CONSTRAINT fk_rates FOREIGN KEY (id_rates) REFERENCES public.rates(id) ON DELETE CASCADE;
 M   ALTER TABLE ONLY public.vehiclehistoryparkingrates DROP CONSTRAINT fk_rates;
       public          postgres    false    258    260    4820            �           2606    74756 #   vehiclehistory fk_registeredvehicle    FK CONSTRAINT     �   ALTER TABLE ONLY public.vehiclehistory
    ADD CONSTRAINT fk_registeredvehicle FOREIGN KEY (id_registeredvehicle) REFERENCES public.registeredvehicle(id) ON DELETE CASCADE;
 M   ALTER TABLE ONLY public.vehiclehistory DROP CONSTRAINT fk_registeredvehicle;
       public          postgres    false    248    236    4798            �           2606    74686    formrolpermission fk_role    FK CONSTRAINT     �   ALTER TABLE ONLY public.formrolpermission
    ADD CONSTRAINT fk_role FOREIGN KEY (id_rol) REFERENCES public.rol(id) ON DELETE CASCADE;
 C   ALTER TABLE ONLY public.formrolpermission DROP CONSTRAINT fk_role;
       public          postgres    false    4774    238    222            �           2606    74708    roluser fk_role    FK CONSTRAINT     }   ALTER TABLE ONLY public.roluser
    ADD CONSTRAINT fk_role FOREIGN KEY (id_rol) REFERENCES public.rol(id) ON DELETE CASCADE;
 9   ALTER TABLE ONLY public.roluser DROP CONSTRAINT fk_role;
       public          postgres    false    4774    240    222            �           2606    74806    rates fk_typerates    FK CONSTRAINT     �   ALTER TABLE ONLY public.rates
    ADD CONSTRAINT fk_typerates FOREIGN KEY (id_typerates) REFERENCES public.typerates(id) ON DELETE CASCADE;
 <   ALTER TABLE ONLY public.rates DROP CONSTRAINT fk_typerates;
       public          postgres    false    258    256    4818            �           2606    74751    vehiclehistory fk_typevehicle    FK CONSTRAINT     �   ALTER TABLE ONLY public.vehiclehistory
    ADD CONSTRAINT fk_typevehicle FOREIGN KEY (id_typevehicle) REFERENCES public.typevehicles(id) ON DELETE CASCADE;
 G   ALTER TABLE ONLY public.vehiclehistory DROP CONSTRAINT fk_typevehicle;
       public          postgres    false    246    248    4808            �           2606    74634    client fk_user    FK CONSTRAINT     �   ALTER TABLE ONLY public.client
    ADD CONSTRAINT fk_user FOREIGN KEY (id_user) REFERENCES public."user"(id) ON DELETE SET NULL;
 8   ALTER TABLE ONLY public.client DROP CONSTRAINT fk_user;
       public          postgres    false    4784    228    230            �           2606    74674    registeredvehicle fk_vehicle    FK CONSTRAINT     �   ALTER TABLE ONLY public.registeredvehicle
    ADD CONSTRAINT fk_vehicle FOREIGN KEY (id_vehicle) REFERENCES public.vehicle(id) ON DELETE CASCADE;
 F   ALTER TABLE ONLY public.registeredvehicle DROP CONSTRAINT fk_vehicle;
       public          postgres    false    236    4794    234            �           2606    74732    membershipsvehicle fk_vehicle    FK CONSTRAINT     �   ALTER TABLE ONLY public.membershipsvehicle
    ADD CONSTRAINT fk_vehicle FOREIGN KEY (id_vehicle) REFERENCES public.vehicle(id) ON DELETE CASCADE;
 G   ALTER TABLE ONLY public.membershipsvehicle DROP CONSTRAINT fk_vehicle;
       public          postgres    false    234    4794    244            �           2606    74787    invoice fk_vehiclehistory    FK CONSTRAINT     �   ALTER TABLE ONLY public.invoice
    ADD CONSTRAINT fk_vehiclehistory FOREIGN KEY (id_vehiclehistory) REFERENCES public.vehiclehistory(id) ON DELETE CASCADE;
 C   ALTER TABLE ONLY public.invoice DROP CONSTRAINT fk_vehiclehistory;
       public          postgres    false    254    4810    248            �           2606    74823 ,   vehiclehistoryparkingrates fk_vehiclehistory    FK CONSTRAINT     �   ALTER TABLE ONLY public.vehiclehistoryparkingrates
    ADD CONSTRAINT fk_vehiclehistory FOREIGN KEY (id_vehiclehistory) REFERENCES public.vehiclehistory(id) ON DELETE CASCADE;
 V   ALTER TABLE ONLY public.vehiclehistoryparkingrates DROP CONSTRAINT fk_vehiclehistory;
       public          postgres    false    4810    260    248            �           2606    82624 %   vehiclehistory fk_vehiclehistory_type    FK CONSTRAINT     �   ALTER TABLE ONLY public.vehiclehistory
    ADD CONSTRAINT fk_vehiclehistory_type FOREIGN KEY (id_invoice) REFERENCES public.invoice(id) ON DELETE CASCADE;
 O   ALTER TABLE ONLY public.vehiclehistory DROP CONSTRAINT fk_vehiclehistory_type;
       public          postgres    false    4816    248    254            �   >   x�34�LI�JT�IT��/�W(�,JLI�4202�50�52�4�,�24��G6�L����� O�      �      x���,Ì��D�+F��� Kt�      �   $   x�3���L-*���,�2��S�rA�=... ���      �      x�3�4�4�,����� ��      �   !   x�34�4�4�4�,�24�429Ӹb���� 85�         R   x�M�=
�0���)<����a\I�P�����Wpq}oŭV���C�iCdv���e�l�_�Ѵ)B��+C��Dǿ;�3=e^1      �      x������ � �      �   .   x�3�,N�M�K��4202�50�54@f�p�`�02Ff�q��qqq u�      �      x������ � �      }   "   x�3�,�8K��83�s9ӸL!��=... P@�      �   )   x�34�L,N#�┴�ĴN��D 3%1���+F��� �jH      �   M   x�3�,N��,�,�2�t.JM�L9�KJ��U`��kNfnf�c��ZQ�_T�p���q�qYpz�BEӸb���� �Vc      �   �   x�m�Kj�0D���c�[���r�l�Q��=��}�JB�ZPUO%�[���G�e���^H�2 ���)�>��[�1�֚%�����
1+�,��[<��Zxl>��?&�H�T���z�}���On��Ƭbm���Y��5��{(Ðb��B)�͖ة䌳Zь^W�y��vI	SZx�ӆ�|�I�bM?�S���o�4}~2c�      �   %   x�3�450�30�40�20 "N#N3(��+F��� rk      �      x������ � �      �   c   x�-�K
�0E�q�������+pNB�@�m����
�<.g��J���1&���h!P��F��<؇�>��q����lu���i������\9'D|�#�      �   !   x�3�4�4�,�2�4���@�$���� G��      �   Z   x�3���O�H�4�30�,�2�t9�6��X��3�t��SHIU(N�M�K�4�(2E�c����T=�(z�k24D�eh�����=... �6�      �      x�3��MU(˯T�,����� (�      �      x������ � �      �      x�3�,NLI,N�,JM�4�,����� H"a      �      x������ � �      �      x������ � �     