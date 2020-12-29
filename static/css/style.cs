@charset "UTF-8";
* {
  margin: 0px;
  padding: 0px;
  font-family: "DIN Alternate"; }

html {
  height: 100%; }
  html body {
    height: 100%; }

/* -------------------- Font-size -------------------- */
/* iPhone */
/* iPad */
/* iPhone X */
/* -------------------- Base -------------------- */
html {
  font-size: 10px;
  font-family: sans-serif;
  overflow: auto; }

body {
  font-size: 1.8rem;
  overflow: auto; }

@media screen and (min-width: 0px) and (max-width: 320px) {
  h1 {
    font-size: 3.0rem; } }
@media screen and (min-width: 321px) and (max-width: 375px) {
  h1 {
    font-size: 3.0rem; } }
@media screen and (min-width: 376px) and (max-width: 414px) {
  h1 {
    font-size: 3.0rem; } }
@media screen and (min-width: 415px) and (max-width: 768px) {
  h1 {
    font-size: 4.0rem; } }
@media screen and (min-width: 769px) and (max-width: 1024px) {
  h1 {
    font-size: 4.0rem; } }
@media screen and (min-width: 1025px) {
  h1 {
    font-size: 4.0rem; } }

@media screen and (min-width: 0px) and (max-width: 320px) {
  h2 {
    font-size: 2.8rem; } }
@media screen and (min-width: 321px) and (max-width: 375px) {
  h2 {
    font-size: 2.8rem; } }
@media screen and (min-width: 376px) and (max-width: 414px) {
  h2 {
    font-size: 2.8rem; } }
@media screen and (min-width: 415px) and (max-width: 768px) {
  h2 {
    font-size: 3.2rem; } }
@media screen and (min-width: 769px) and (max-width: 1024px) {
  h2 {
    font-size: 3.2rem; } }
@media screen and (min-width: 1025px) {
  h2 {
    font-size: 3.2rem; } }

@media screen and (min-width: 0px) and (max-width: 320px) {
  h3 {
    font-size: 2.4rem; } }
@media screen and (min-width: 321px) and (max-width: 375px) {
  h3 {
    font-size: 2.4rem; } }
@media screen and (min-width: 376px) and (max-width: 414px) {
  h3 {
    font-size: 2.4rem; } }
@media screen and (min-width: 415px) and (max-width: 768px) {
  h3 {
    font-size: 2.8rem; } }
@media screen and (min-width: 769px) and (max-width: 1024px) {
  h3 {
    font-size: 2.8rem; } }
@media screen and (min-width: 1025px) {
  h3 {
    font-size: 2.8rem; } }

@media screen and (min-width: 0px) and (max-width: 320px) {
  h4 {
    font-size: 2.0rem; } }
@media screen and (min-width: 321px) and (max-width: 375px) {
  h4 {
    font-size: 2.0rem; } }
@media screen and (min-width: 376px) and (max-width: 414px) {
  h4 {
    font-size: 2.0rem; } }
@media screen and (min-width: 415px) and (max-width: 768px) {
  h4 {
    font-size: 2.4rem; } }
@media screen and (min-width: 769px) and (max-width: 1024px) {
  h4 {
    font-size: 2.4rem; } }
@media screen and (min-width: 1025px) {
  h4 {
    font-size: 2.4rem; } }

@media screen and (min-width: 0px) and (max-width: 320px) {
  h5 {
    font-size: 1.6rem; } }
@media screen and (min-width: 321px) and (max-width: 375px) {
  h5 {
    font-size: 1.6rem; } }
@media screen and (min-width: 376px) and (max-width: 414px) {
  h5 {
    font-size: 1.6rem; } }
@media screen and (min-width: 415px) and (max-width: 768px) {
  h5 {
    font-size: 2.0rem; } }
@media screen and (min-width: 769px) and (max-width: 1024px) {
  h5 {
    font-size: 2.0rem; } }
@media screen and (min-width: 1025px) {
  h5 {
    font-size: 2.0rem; } }

/* -------------------- Color-variables -------------------- */
/* -------------------- navbar -------------------- */
.navbar-wrapper {
  z-index: 30;
  top: 0;
  position: fixed;
  width: 100%;
  height: 60px;
  background-color: #1c1c1e;
  opacity: 0.9;
  display: flex; }
  .navbar-wrapper .navbar-logo {
    display: block;
    margin-top: auto;
    margin-bottom: auto;
    margin-left: 100px;
    height: 31.94px; }
    .navbar-wrapper .navbar-logo img {
      width: 32px; }
  .navbar-wrapper .navbar-list {
    margin: 0 0 0 auto; }
    .navbar-wrapper .navbar-list .navbar-item {
      margin-right: 140px; }
    .navbar-wrapper .navbar-list ul {
      font-size: 24px; }
  @media screen and (min-width: 0px) and (max-width: 320px) {
    .navbar-wrapper {
      height: 40px; }
      .navbar-wrapper .navbar-logo {
        margin-left: 32px;
        height: 19.95px; }
        .navbar-wrapper .navbar-logo img {
          width: 20px; }
      .navbar-wrapper .navbar-list {
        margin: 0 0 0 auto; }
        .navbar-wrapper .navbar-list .navbar-item {
          margin-right: 32px; }
          .navbar-wrapper .navbar-list .navbar-item a {
            line-height: 40px;
            color: #CDCDCD;
            text-decoration: none; }
            .navbar-wrapper .navbar-list .navbar-item a:hover {
              color: #FDFDFD; }
        .navbar-wrapper .navbar-list ul {
          display: flex;
          font-size: 16px;
          list-style: none; } }
  @media screen and (min-width: 321px) and (max-width: 414px) {
    .navbar-wrapper {
      height: 40px; }
      .navbar-wrapper .navbar-logo {
        margin-left: 40px;
        height: 19.95px; }
        .navbar-wrapper .navbar-logo img {
          width: 20px; }
      .navbar-wrapper .navbar-list {
        margin: 0 0 0 auto; }
        .navbar-wrapper .navbar-list .navbar-item {
          margin-right: 40px; }
          .navbar-wrapper .navbar-list .navbar-item a {
            line-height: 40px;
            color: #CDCDCD;
            text-decoration: none; }
            .navbar-wrapper .navbar-list .navbar-item a:hover {
              color: #FDFDFD; }
        .navbar-wrapper .navbar-list ul {
          display: flex;
          font-size: 16px;
          list-style: none; } }
  @media screen and (min-width: 415px) and (max-width: 560px) {
    .navbar-wrapper {
      height: 40px; }
      .navbar-wrapper .navbar-logo {
        margin-left: 40px;
        height: 23.95px; }
        .navbar-wrapper .navbar-logo img {
          width: 24px; }
      .navbar-wrapper .navbar-list {
        margin: 0 0 0 auto; }
        .navbar-wrapper .navbar-list .navbar-item {
          margin-right: 60px; }
          .navbar-wrapper .navbar-list .navbar-item a {
            line-height: 40px;
            color: #CDCDCD;
            text-decoration: none; }
            .navbar-wrapper .navbar-list .navbar-item a:hover {
              color: #FDFDFD; }
        .navbar-wrapper .navbar-list ul {
          display: flex;
          font-size: 20px;
          list-style: none; } }
  @media screen and (min-width: 561px) and (max-width: 1024px) {
    .navbar-wrapper .navbar-logo {
      margin-left: 80px; }
    .navbar-wrapper .navbar-list .navbar-item {
      margin-right: 100px; } }
  @media screen and (min-width: 561px) {
    .navbar-wrapper ul {
      display: flex;
      list-style-type: none;
      font-size: 20px; }
      .navbar-wrapper ul a {
        line-height: 60px;
        color: #CDCDCD;
        text-decoration: none; }
        .navbar-wrapper ul a:hover {
          color: #FDFDFD; } }

/* -------------------- thumbnail -------------------- */
.thumbnail {
  background-color: #efefef;
  min-height: 80%;
  height: 90%; }
  @media screen and (min-width: 0px) and (max-width: 320px) {
    .thumbnail {
      min-height: 0%;
      height: 36%; } }
  @media screen and (min-width: 321px) and (max-width: 414px) {
    .thumbnail {
      min-height: 0%;
      height: 36%; } }
  @media screen and (min-width: 415px) and (max-width: 560px) {
    .thumbnail {
      min-height: 0%;
      height: 40%; } }
  @media screen and (min-width: 561px) and (max-width: 1024px) {
    .thumbnail {
      min-height: 0%;
      height: 44%; } }

/* -------------------- dividing-line -------------------- */
hr {
  height: 2px;
  width: 70%;
  background-color: #C4C4C4;
  border: 0;
  margin: auto; }

/* -------------------- entry -------------------- */
.index {
  background-image: url("../images/HP_thumbnail_index.png");
  background-repeat: no-repeat;
  background-size: 100% auto;
  background-attachment: fixed; }
  @media screen and (min-width: 0px) and (max-width: 560px) {
    .index {
      background-position: 0px 40px;
      background-attachment: scroll; } }
  @media screen and (min-width: 561px) and (max-width: 1024px) {
    .index {
      background-attachment: scroll; } }

.entry {
  text-align: center;
  background-color: #efefef;
  padding-bottom: 100px; }
  .entry h2 {
    margin-bottom: 40px;
    color: #505050; }

@media screen and (min-width: 0px) and (max-width: 414px) {
  .entry {
    padding-bottom: 60px; } }
/* -------------------- about -------------------- */
.about {
  padding-top: 100px;
  margin-bottom: 200px;
  display: block; }
  .about .about-contents {
    display: flex;
    width: max-content;
    margin-left: auto;
    margin-right: auto;
    padding: 40px; }
    .about .about-contents .profile-icon {
      max-width: 50%;
      margin-right: 100px; }
      .about .about-contents .profile-icon .profile-img {
        max-width: 300px;
        width: 18vw;
        border-radius: 100vw;
        display: block; }
        @media screen and (min-width: 0px) and (max-width: 560px) {
          .about .about-contents .profile-icon .profile-img {
            width: 30vw;
            border-radius: 100vw; } }
    .about .about-contents .about-texts {
      height: 18vw; }
      .about .about-contents .about-texts .texts-contents {
        height: 18vw;
        display: table-cell;
        text-align: center;
        vertical-align: middle;
        text-align: left; }
        .about .about-contents .about-texts .texts-contents h3 {
          color: #505050; }
        .about .about-contents .about-texts .texts-contents p {
          color: #707070;
          margin-bottom: 10px; }
        .about .about-contents .about-texts .texts-contents .sns-icon {
          margin-top: 24px; }
          .about .about-contents .about-texts .texts-contents .sns-icon img {
            width: 32px;
            height: 32px;
            margin-right: 28px; }
          @media screen and (max-width: 560px) {
            .about .about-contents .about-texts .texts-contents .sns-icon {
              margin-top: 30px;
              margin-bottom: 50px; }
              .about .about-contents .about-texts .texts-contents .sns-icon img {
                width: 30px;
                height: 30px;
                margin-right: 10px;
                margin-left: 10px; } }

@media screen and (min-width: 0px) and (max-width: 320px) {
  .about {
    padding-top: 60px; }
    .about h2 {
      font-size: 20px;
      margin-bottom: 28px; }
    .about .about-contents {
      display: block;
      padding: 0; }
      .about .about-contents .profile-icon {
        margin-top: 0;
        margin-bottom: 16px;
        margin-right: auto;
        margin-left: auto; }
        .about .about-contents .profile-icon .profile-img {
          margin: 0 auto; }
      .about .about-contents .about-texts .texts-contents {
        text-align: center; }
        .about .about-contents .about-texts .texts-contents h3 {
          font-size: 16px; }
        .about .about-contents .about-texts .texts-contents p {
          font-size: 12px; } }
@media screen and (min-width: 321px) and (max-width: 414px) {
  .about {
    padding-top: 80px; }
    .about h2 {
      font-size: 24px;
      margin-bottom: 28px; }
    .about .about-contents {
      display: block;
      padding: 0; }
      .about .about-contents .profile-icon {
        margin-top: 0;
        margin-bottom: 16px;
        margin-right: auto;
        margin-left: auto; }
        .about .about-contents .profile-icon .profile-img {
          margin: 0 auto; }
      .about .about-contents .about-texts .texts-contents {
        text-align: center; }
        .about .about-contents .about-texts .texts-contents h3 {
          font-size: 16px; }
        .about .about-contents .about-texts .texts-contents p {
          font-size: 12px; } }
@media screen and (min-width: 415px) and (max-width: 560px) {
  .about {
    padding-top: 100px; }
    .about h2 {
      font-size: 24px;
      margin-bottom: 28px; }
    .about .about-contents {
      display: block;
      padding: 0; }
      .about .about-contents .profile-icon {
        max-width: 100%;
        margin-top: 0;
        margin-bottom: 16px;
        margin-left: 0;
        margin-right: 0; }
        .about .about-contents .profile-icon .profile-img {
          margin: 0 auto; }
      .about .about-contents .about-texts .texts-contents {
        text-align: center; }
        .about .about-contents .about-texts .texts-contents h3 {
          font-size: 20px; }
        .about .about-contents .about-texts .texts-contents p {
          font-size: 14px; } }
@media screen and (min-width: 561px) and (max-width: 1024px) {
  .about {
    padding-top: 120px; }
    .about h2 {
      margin-bottom: 40px; }
    .about .about-contents {
      padding: 0; }
      .about .about-contents .profile-icon {
        margin: auto 0;
        padding-right: 80px; }
        .about .about-contents .profile-icon .profile-img {
          margin: 0 auto; }
      .about .about-contents .about-texts {
        height: fit-content; }
        .about .about-contents .about-texts .texts-contents h3 {
          font-size: 24px; }
        .about .about-contents .about-texts .texts-contents p {
          font-size: 16px; } }
/* -------------------- skills -------------------- */
.skills {
  margin-bottom: 200px; }
  .skills .skills-table {
    max-width: 1060px;
    margin: 0 auto;
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    justify-content: center; }
    .skills .skills-table .skills-contents {
      border: solid 1px #bcbcbc;
      border-radius: 15px;
      margin-right: 20px;
      margin-left: 20px;
      margin-top: 20px;
      margin-bottom: 20px;
      min-width: 200px;
      padding-top: 20px;
      padding-bottom: 20px;
      padding-right: 10px;
      padding-left: 10px; }
      .skills .skills-table .skills-contents .skills-name {
        color: #505050; }
      .skills .skills-table .skills-contents .star {
        margin-top: 4px;
        margin-bottom: 4px; }
        .skills .skills-table .skills-contents .star img {
          width: 150px;
          height: 20px; }
      .skills .skills-table .skills-contents .skills-text {
        max-width: 164px;
        margin: 0 auto;
        font-size: 12px;
        color: #707070; }
    .skills .skills-table img {
      width: 60px;
      height: 60px; }

@media screen and (min-width: 0px) and (max-width: 320px) {
  .skills {
    margin-bottom: 80px; }
    .skills h2 {
      font-size: 20px;
      margin-bottom: 28px; }
    .skills .skills-table .skills-contents {
      min-width: 120px;
      min-height: 160px;
      margin-right: 4px;
      margin-left: 4px;
      margin-top: 8px;
      margin-bottom: 8px;
      padding-top: 10px;
      padding-bottom: 10px;
      padding-right: 4px;
      padding-left: 4px; }
      .skills .skills-table .skills-contents img {
        width: 40px;
        height: 40px; }
      .skills .skills-table .skills-contents .skills-name {
        font-size: 14px; }
      .skills .skills-table .skills-contents .star {
        margin-top: 4px;
        margin-bottom: 4px; }
        .skills .skills-table .skills-contents .star img {
          width: 112.5px;
          height: 15px; }
      .skills .skills-table .skills-contents .skills-text {
        max-width: 100px;
        font-size: 8px; } }
@media screen and (min-width: 321px) and (max-width: 414px) {
  .skills {
    margin-bottom: 80px; }
    .skills h2 {
      font-size: 24px;
      margin-bottom: 28px; }
    .skills .skills-table .skills-contents {
      min-width: 120px;
      margin-right: 4px;
      margin-left: 4px;
      margin-top: 8px;
      margin-bottom: 8px;
      padding-top: 10px;
      padding-bottom: 10px;
      padding-right: 8px;
      padding-left: 8px; }
      .skills .skills-table .skills-contents img {
        width: 40px;
        height: 40px; }
      .skills .skills-table .skills-contents .skills-name {
        font-size: 14px; }
      .skills .skills-table .skills-contents .star {
        margin-top: 4px;
        margin-bottom: 4px; }
        .skills .skills-table .skills-contents .star img {
          width: 112.5px;
          height: 15px; }
      .skills .skills-table .skills-contents .skills-text {
        max-width: 140px;
        font-size: 8px; } }
@media screen and (min-width: 415px) and (max-width: 560px) {
  .skills {
    margin-bottom: 80px; }
    .skills h2 {
      font-size: 24px;
      margin-bottom: 28px; }
    .skills .skills-table .skills-contents {
      margin: 10px; } }
@media screen and (min-width: 561px) and (max-width: 1024px) {
  .skills {
    margin-bottom: 120px; }
    .skills h2 {
      margin-bottom: 40px; }
    .skills .skills-table .skills-contents {
      margin: 10px; } }
/* -------------------- works -------------------- */
.works .works-table {
  max-width: 1060px;
  margin: 0 auto; }
  .works .works-table .works-contents {
    width: fit-content;
    display: flex;
    margin: 0 auto; }
    .works .works-table .works-contents a {
      display: inline-block;
      text-decoration: none; }
      .works .works-table .works-contents a img {
        width: 320px; }
        .works .works-table .works-contents a img:hover {
          opacity: 0.6; }
      .works .works-table .works-contents a h3 {
        margin-top: 20px;
        margin-bottom: 20px;
        color: #505050; }
    .works .works-table .works-contents .works-content {
      margin-right: 40px;
      margin-left: 40px;
      margin-bottom: 80px; }
      .works .works-table .works-contents .works-content p {
        color: #707070; }
        .works .works-table .works-contents .works-content p:hover {
          color: #000000; }
      .works .works-table .works-contents .works-content .works-img {
        height: 320px;
        background-color: #000; }

@media screen and (min-width: 0px) and (max-width: 320px) {
  .works h2 {
    font-size: 20px;
    margin-bottom: 28px; }
  .works .works-table .works-contents {
    display: block; }
    .works .works-table .works-contents .works-content {
      margin-bottom: 44px; }
      .works .works-table .works-contents .works-content .works-img {
        height: 200px; }
        .works .works-table .works-contents .works-content .works-img img {
          width: 200px;
          height: 200px; }
      .works .works-table .works-contents .works-content .works-contents-text h3 {
        font-size: 18px;
        margin-bottom: 10px; }
      .works .works-table .works-contents .works-content .works-contents-text p {
        font-size: 14px; } }
@media screen and (min-width: 321px) and (max-width: 414px) {
  .works h2 {
    font-size: 24px;
    margin-bottom: 28px; }
  .works .works-table .works-contents {
    display: block; }
    .works .works-table .works-contents .works-content {
      margin-bottom: 44px; }
      .works .works-table .works-contents .works-content .works-img {
        height: 240px; }
        .works .works-table .works-contents .works-content .works-img img {
          width: 240px;
          height: 240px; }
      .works .works-table .works-contents .works-content .works-contents-text h3 {
        font-size: 18px;
        margin-bottom: 10px; }
      .works .works-table .works-contents .works-content .works-contents-text p {
        font-size: 14px; } }
@media screen and (min-width: 415px) and (max-width: 560px) {
  .works h2 {
    font-size: 24px;
    margin-bottom: 28px; }
  .works .works-table .works-contents .works-content {
    margin-left: 16px;
    margin-right: 16px;
    margin-bottom: 0px; }
    .works .works-table .works-contents .works-content .works-img {
      height: 180px; }
      .works .works-table .works-contents .works-content .works-img img {
        width: 180px;
        height: 180px; }
    .works .works-table .works-contents .works-content .works-contents-text h3 {
      font-size: 20px;
      margin-bottom: 10px; }
    .works .works-table .works-contents .works-content .works-contents-text p {
      font-size: 14px; } }
@media screen and (min-width: 561px) and (max-width: 1024px) {
  .works h2 {
    margin-bottom: 44px; }
  .works .works-table .works-contents .works-content {
    margin-left: 20px;
    margin-right: 20px;
    margin-bottom: 0px; }
    .works .works-table .works-contents .works-content .works-img {
      height: 240px; }
      .works .works-table .works-contents .works-content .works-img img {
        width: 240px;
        height: 240px; }
    .works .works-table .works-contents .works-content .works-contents-text h3 {
      font-size: 24px;
      margin-bottom: 10px; }
    .works .works-table .works-contents .works-content .works-contents-text p {
      font-size: 16px; } }
/* -------------------- footer -------------------- */
.footer-border {
  height: 1px;
  width: 100%;
  border: 0; }

.footer-wrapper {
  width: 100%;
  height: 60px;
  display: flex;
  background-color: #efefef;
  padding-top: 10px;
  padding-bottom: 10px; }
  .footer-wrapper .footer-left {
    width: 50%;
    margin-left: 80px;
    display: flex; }
    .footer-wrapper .footer-left .footer-logo {
      height: 39.92px;
      display: block;
      margin-top: auto;
      margin-bottom: auto; }
      .footer-wrapper .footer-left .footer-logo img {
        width: 40px; }
    .footer-wrapper .footer-left p {
      line-height: 60px;
      margin-left: 40px;
      font-size: 20px;
      color: #909090; }
  .footer-wrapper .footer-right {
    width: 50%;
    text-align: right;
    margin-right: 80px; }
    .footer-wrapper .footer-right .contact a {
      font-size: 24px;
      text-decoration: none;
      line-height: 60px;
      color: #909090; }
      .footer-wrapper .footer-right .contact a:hover {
        color: #707070; }
  @media screen and (min-width: 0px) and (max-width: 320px) {
    .footer-wrapper {
      height: 28px; }
      .footer-wrapper .footer-left {
        margin-left: 28px; }
        .footer-wrapper .footer-left .footer-logo {
          height: 31.94px; }
          .footer-wrapper .footer-left .footer-logo img {
            width: 28px; }
        .footer-wrapper .footer-left p {
          display: none; }
      .footer-wrapper .footer-right {
        text-align: right;
        width: 38%;
        margin-right: 28px; }
        .footer-wrapper .footer-right .contact a {
          font-size: 16px;
          line-height: 28px; } }
  @media screen and (min-width: 321px) and (max-width: 414px) {
    .footer-wrapper {
      height: 40px; }
      .footer-wrapper .footer-left {
        margin-left: 28px; }
        .footer-wrapper .footer-left .footer-logo {
          height: 31.94px; }
          .footer-wrapper .footer-left .footer-logo img {
            width: 32px; }
        .footer-wrapper .footer-left p {
          font-size: 12px;
          margin-left: 16px;
          line-height: 40px; }
      .footer-wrapper .footer-right {
        text-align: right;
        width: 40%;
        margin-right: 28px; }
        .footer-wrapper .footer-right .contact a {
          font-size: 20px;
          line-height: 40px; } }
  @media screen and (min-width: 415px) and (max-width: 560px) {
    .footer-wrapper {
      height: 40px; }
      .footer-wrapper .footer-left {
        margin-left: 40px; }
        .footer-wrapper .footer-left .footer-logo {
          height: 31.94px; }
          .footer-wrapper .footer-left .footer-logo img {
            width: 32px; }
        .footer-wrapper .footer-left p {
          font-size: 12px;
          margin-left: 20px;
          line-height: 40px; }
      .footer-wrapper .footer-right {
        margin-right: 40px; }
        .footer-wrapper .footer-right .contact a {
          font-size: 20px;
          line-height: 40px; } }
  @media screen and (min-width: 561px) and (max-width: 1024px) {
    .footer-wrapper .footer-left {
      margin-left: 60px; }
      .footer-wrapper .footer-left p {
        font-size: 16px;
        margin-left: 20px;
        line-height: 60px; }
    .footer-wrapper .footer-right {
      margin-right: 60px; } }

/* -------------------- art -------------------- */
.art {
  background-image: url("../images/HP_thumbnail_art.png");
  background-repeat: no-repeat;
  background-size: 100% auto;
  background-attachment: fixed; }
  @media screen and (min-width: 0px) and (max-width: 560px) {
    .art {
      background-position: 0px 40px;
      background-attachment: scroll; } }
  @media screen and (min-width: 561px) and (max-width: 1024px) {
    .art {
      background-attachment: scroll; } }

.article-comp {
  padding-bottom: 180px;
  background-color: #efefef; }
  .article-comp .article-layer {
    width: 100%;
    text-align: center;
    background-color: #efefef; }
    .article-comp .article-layer .thumbnail_art img {
      width: 100%; }
    .article-comp .article-layer .article-block {
      margin-top: 180px;
      display: inline-block;
      max-width: 1060px;
      text-align: center; }
      .article-comp .article-layer .article-block h2 {
        margin-bottom: 60px;
        color: #505050; }
      .article-comp .article-layer .article-block .article-table {
        width: 100%;
        overflow: hidden; }
        .article-comp .article-layer .article-block .article-table a {
          position: relative;
          margin: 30px;
          display: inline-block;
          background-color: #000; }
          .article-comp .article-layer .article-block .article-table a img {
            width: 260px;
            height: 260px;
            object-fit: cover;
            vertical-align: bottom; }
            .article-comp .article-layer .article-block .article-table a img:hover {
              opacity: 0.6; }
          .article-comp .article-layer .article-block .article-table a .illustration-name {
            background-color: black;
            width: 260px;
            height: 260px;
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            -webkit-transform: translate(-50%, -50%);
            opacity: 0.0; }
            .article-comp .article-layer .article-block .article-table a .illustration-name p {
              color: white;
              width: 100px;
              position: absolute;
              top: 50%;
              left: 50%;
              transform: translate(-50%, -50%);
              -webkit-transform: translate(-50%, -50%);
              opacity: 0.0; }
            .article-comp .article-layer .article-block .article-table a .illustration-name:hover {
              opacity: 0.6; }
              .article-comp .article-layer .article-block .article-table a .illustration-name:hover p {
                opacity: 1.0; }
        @media screen and (min-width: 0px) and (max-width: 320px) {
          .article-comp .article-layer .article-block .article-table a {
            margin-top: 0px;
            margin-bottom: 4px;
            margin-left: 0px;
            margin-right: 0px; }
            .article-comp .article-layer .article-block .article-table a img {
              width: 100px;
              height: 100px; }
            .article-comp .article-layer .article-block .article-table a .illustration-name {
              width: 100px;
              height: 100px;
              display: none; }
              .article-comp .article-layer .article-block .article-table a .illustration-name p {
                display: none; } }
        @media screen and (min-width: 321px) and (max-width: 414px) {
          .article-comp .article-layer .article-block .article-table a {
            margin-top: 0px;
            margin-bottom: 4px;
            margin-left: 0px;
            margin-right: 0px; }
            .article-comp .article-layer .article-block .article-table a img {
              width: 116px;
              height: 116px; }
            .article-comp .article-layer .article-block .article-table a .illustration-name {
              width: 116px;
              height: 116px;
              display: none; }
              .article-comp .article-layer .article-block .article-table a .illustration-name p {
                display: none; } }
        @media screen and (min-width: 415px) and (max-width: 560px) {
          .article-comp .article-layer .article-block .article-table {
            max-width: 520px; }
            .article-comp .article-layer .article-block .article-table a {
              margin: 4px; }
              .article-comp .article-layer .article-block .article-table a img {
                width: 140px;
                height: 140px; }
              .article-comp .article-layer .article-block .article-table a .illustration-name {
                width: 140px;
                height: 140px; }
                .article-comp .article-layer .article-block .article-table a .illustration-name p {
                  font-size: 16px; } }
        @media screen and (min-width: 561px) and (max-width: 1024px) {
          .article-comp .article-layer .article-block .article-table {
            max-width: 760px; }
            .article-comp .article-layer .article-block .article-table a {
              margin: 4px; }
              .article-comp .article-layer .article-block .article-table a img {
                width: 180px;
                height: 180px; }
              .article-comp .article-layer .article-block .article-table a .illustration-name {
                width: 180px;
                height: 180px; } }

@media screen and (min-width: 0px) and (max-width: 320px) {
  .article-comp {
    padding-bottom: 60px; }
    .article-comp .article-layer .article-block {
      margin-top: 60px; }
      .article-comp .article-layer .article-block h2 {
        font-size: 20px;
        margin-bottom: 28px; } }
@media screen and (min-width: 321px) and (max-width: 414px) {
  .article-comp {
    padding-bottom: 80px; }
    .article-comp .article-layer .article-block {
      margin-top: 100px; }
      .article-comp .article-layer .article-block h2 {
        font-size: 24px;
        margin-bottom: 32px; } }
@media screen and (min-width: 415px) and (max-width: 560px) {
  .article-comp {
    padding-bottom: 100px; }
    .article-comp .article-layer .article-block {
      margin-top: 120px; }
      .article-comp .article-layer .article-block h2 {
        font-size: 24px;
        margin-bottom: 40px; } }
@media screen and (min-width: 561px) and (max-width: 1024px) {
  .article-comp {
    padding-bottom: 120px; }
    .article-comp .article-layer .article-block {
      margin-top: 140px; }
      .article-comp .article-layer .article-block h2 {
        margin-bottom: 40px; } }
/* -------------------- design --------------------*/
.design {
  background-image: url("../images/HP_thumbnail_design.png");
  background-repeat: no-repeat;
  background-size: 100% auto;
  background-attachment: fixed; }
  @media screen and (min-width: 0px) and (max-width: 560px) {
    .design {
      background-position: 0px 40px;
      background-attachment: scroll; } }
  @media screen and (min-width: 561px) and (max-width: 1024px) {
    .design {
      background-attachment: scroll; } }

.contents-comp {
  padding-bottom: 180px;
  background-color: #efefef; }
  .contents-comp .contents-layer {
    width: 100%;
    text-align: center;
    background-color: #efefef; }
    .contents-comp .contents-layer .contents-block {
      margin-top: 180px;
      display: inline-block;
      max-width: 1060px;
      text-align: center; }
      .contents-comp .contents-layer .contents-block h2 {
        margin-bottom: 60px;
        color: #505050; }
      .contents-comp .contents-layer .contents-block .contents-table {
        width: 100%; }
        .contents-comp .contents-layer .contents-block .contents-table a {
          position: relative;
          margin: 30px;
          display: inline-block;
          text-decoration: none; }
          .contents-comp .contents-layer .contents-block .contents-table a .contents-img {
            background-color: #000; }
            .contents-comp .contents-layer .contents-block .contents-table a .contents-img img {
              width: 260px;
              height: 260px;
              object-fit: cover;
              vertical-align: bottom; }
              .contents-comp .contents-layer .contents-block .contents-table a .contents-img img:hover {
                opacity: 0.6; }
          .contents-comp .contents-layer .contents-block .contents-table a .contents-name {
            margin-top: 28px; }
            .contents-comp .contents-layer .contents-block .contents-table a .contents-name p {
              color: #707070; }
              .contents-comp .contents-layer .contents-block .contents-table a .contents-name p:hover {
                color: #000; }
        @media screen and (min-width: 0px) and (max-width: 320px) {
          .contents-comp .contents-layer .contents-block .contents-table a {
            margin-top: 0px;
            margin-bottom: 20px;
            margin-left: 0px;
            margin-right: 0px; }
            .contents-comp .contents-layer .contents-block .contents-table a .contents-img {
              background-color: none; }
              .contents-comp .contents-layer .contents-block .contents-table a .contents-img img {
                width: 100px;
                height: 100px; }
            .contents-comp .contents-layer .contents-block .contents-table a .contents-name {
              margin-top: 8px; }
              .contents-comp .contents-layer .contents-block .contents-table a .contents-name p {
                font-size: 12px; } }
        @media screen and (min-width: 321px) and (max-width: 414px) {
          .contents-comp .contents-layer .contents-block .contents-table a {
            margin-top: 0px;
            margin-bottom: 24px;
            margin-left: 0px;
            margin-right: 0px; }
            .contents-comp .contents-layer .contents-block .contents-table a .contents-img {
              background-color: none; }
              .contents-comp .contents-layer .contents-block .contents-table a .contents-img img {
                width: 116px;
                height: 116px; }
            .contents-comp .contents-layer .contents-block .contents-table a .contents-name {
              margin-top: 8px; }
              .contents-comp .contents-layer .contents-block .contents-table a .contents-name p {
                font-size: 12px; } }
        @media screen and (min-width: 415px) and (max-width: 560px) {
          .contents-comp .contents-layer .contents-block .contents-table {
            max-width: 520px; }
            .contents-comp .contents-layer .contents-block .contents-table a {
              margin-top: 0px;
              margin-bottom: 24px;
              margin-left: 4px;
              margin-right: 4px; }
              .contents-comp .contents-layer .contents-block .contents-table a .contents-img {
                background-color: none; }
                .contents-comp .contents-layer .contents-block .contents-table a .contents-img img {
                  width: 140px;
                  height: 140px; }
              .contents-comp .contents-layer .contents-block .contents-table a .contents-name {
                margin-top: 8px; }
                .contents-comp .contents-layer .contents-block .contents-table a .contents-name p {
                  font-size: 16px; } }
        @media screen and (min-width: 561px) and (max-width: 1024px) {
          .contents-comp .contents-layer .contents-block .contents-table {
            max-width: 760px; }
            .contents-comp .contents-layer .contents-block .contents-table a {
              margin-top: 0px;
              margin-bottom: 24px;
              margin-left: 4px;
              margin-right: 4px; }
              .contents-comp .contents-layer .contents-block .contents-table a .contents-img {
                background-color: none; }
                .contents-comp .contents-layer .contents-block .contents-table a .contents-img img {
                  width: 180px;
                  height: 180px; }
              .contents-comp .contents-layer .contents-block .contents-table a .contents-name {
                margin-top: 8px; }
                .contents-comp .contents-layer .contents-block .contents-table a .contents-name p {
                  font-size: 16px; } }

@media screen and (min-width: 0px) and (max-width: 320px) {
  .contents-comp {
    padding-bottom: 60px; }
    .contents-comp .contents-layer .contents-block {
      margin-top: 60px; }
      .contents-comp .contents-layer .contents-block h2 {
        font-size: 20px;
        margin-bottom: 28px; } }
@media screen and (min-width: 321px) and (max-width: 414px) {
  .contents-comp {
    padding-bottom: 80px; }
    .contents-comp .contents-layer .contents-block {
      margin-top: 100px; }
      .contents-comp .contents-layer .contents-block h2 {
        font-size: 24px;
        margin-bottom: 32px; } }
@media screen and (min-width: 415px) and (max-width: 560px) {
  .contents-comp {
    padding-bottom: 100px; }
    .contents-comp .contents-layer .contents-block {
      margin-top: 120px; }
      .contents-comp .contents-layer .contents-block h2 {
        font-size: 24px;
        margin-bottom: 40px; } }
@media screen and (min-width: 561px) and (max-width: 1024px) {
  .contents-comp {
    padding-bottom: 120px; }
    .contents-comp .contents-layer .contents-block {
      margin-top: 140px; }
      .contents-comp .contents-layer .contents-block h2 {
        margin-bottom: 40px; } }
/* -------------------- design個別ページ --------------------*/
.background-color {
  background-color: #efefef; }
  .background-color .contents-entire {
    max-width: 70%;
    margin: 0 auto;
    padding-top: 180px;
    padding-bottom: 180px;
    display: flex; }
    .background-color .contents-entire .iphone-title {
      display: none; }
    .background-color .contents-entire .contents-left {
      width: 600px; }
      .background-color .contents-entire .contents-left img {
        width: 600px;
        margin-bottom: 60px; }
    .background-color .contents-entire .contents-right {
      margin-left: 100px; }
      .background-color .contents-entire .contents-right h3 {
        font-size: 24px;
        color: #505050;
        margin-bottom: 16px; }
      .background-color .contents-entire .contents-right h4 {
        font-size: 20px;
        color: #505050;
        margin-bottom: 8px; }
      .background-color .contents-entire .contents-right p {
        font-size: 16px;
        color: #707070; }

@media screen and (min-width: 0px) and (max-width: 320px) {
  .background-color .contents-entire {
    max-width: 100%;
    display: block;
    text-align: center;
    padding-top: 68px;
    padding-bottom: 60px; }
    .background-color .contents-entire .iphone-title {
      display: inline; }
      .background-color .contents-entire .iphone-title h3 {
        max-width: 80%;
        margin: 0 auto;
        font-size: 16px;
        margin-bottom: 28px;
        color: #505050; }
    .background-color .contents-entire .contents-left {
      width: 100%; }
      .background-color .contents-entire .contents-left a {
        display: inline-block;
        position: relative;
        width: 256px; }
        .background-color .contents-entire .contents-left a img {
          width: 256px;
          object-fit: cover;
          vertical-align: bottom;
          margin-bottom: 20px; }
    .background-color .contents-entire .contents-right {
      max-width: 80%;
      margin: 0 auto; }
      .background-color .contents-entire .contents-right h3 {
        display: none; }
      .background-color .contents-entire .contents-right h4 {
        font-size: 14px;
        color: #505050;
        margin-bottom: 8px; }
      .background-color .contents-entire .contents-right p {
        font-size: 12px;
        color: #707070; } }
@media screen and (min-width: 321px) and (max-width: 414px) {
  .background-color .contents-entire {
    max-width: 100%;
    display: block;
    text-align: center;
    padding-top: 80px;
    padding-bottom: 80px; }
    .background-color .contents-entire .iphone-title {
      display: inline; }
      .background-color .contents-entire .iphone-title h3 {
        max-width: 80%;
        margin: 0 auto;
        font-size: 16px;
        margin-bottom: 28px;
        color: #505050; }
    .background-color .contents-entire .contents-left {
      width: 100%; }
      .background-color .contents-entire .contents-left a {
        display: inline-block;
        position: relative;
        width: 256px; }
        .background-color .contents-entire .contents-left a img {
          width: 256px;
          object-fit: cover;
          vertical-align: bottom;
          margin-bottom: 20px; }
    .background-color .contents-entire .contents-right {
      max-width: 80%;
      margin: 0 auto; }
      .background-color .contents-entire .contents-right h3 {
        display: none; }
      .background-color .contents-entire .contents-right h4 {
        font-size: 14px;
        color: #505050;
        margin-bottom: 8px; }
      .background-color .contents-entire .contents-right p {
        font-size: 12px;
        color: #707070; } }
@media screen and (min-width: 415px) and (max-width: 1260px) {
  .background-color .contents-entire {
    max-width: 100%;
    display: block;
    text-align: center;
    padding-top: 100px;
    padding-bottom: 100px; }
    .background-color .contents-entire .iphone-title {
      display: inline; }
      .background-color .contents-entire .iphone-title h3 {
        max-width: 80%;
        margin: 0 auto;
        font-size: 20px;
        margin-bottom: 28px;
        color: #505050; }
    .background-color .contents-entire .contents-left {
      width: 100%; }
      .background-color .contents-entire .contents-left a {
        display: inline-block;
        position: relative;
        width: 400px; }
        .background-color .contents-entire .contents-left a img {
          width: 400px;
          object-fit: cover;
          vertical-align: bottom;
          margin-bottom: 20px; }
    .background-color .contents-entire .contents-right {
      max-width: 80%;
      margin: 0 auto; }
      .background-color .contents-entire .contents-right h3 {
        display: none; }
      .background-color .contents-entire .contents-right h4 {
        font-size: 16px;
        color: #505050;
        margin-bottom: 8px; }
      .background-color .contents-entire .contents-right p {
        font-size: 14px;
        color: #707070; } }
@media screen and (min-width: 561px) and (max-width: 1024px) {
  .background-color .contents-entire {
    max-width: 100%;
    display: block;
    text-align: center;
    padding-top: 120px;
    padding-bottom: 120px; }
    .background-color .contents-entire .iphone-title {
      display: inline; }
      .background-color .contents-entire .iphone-title h3 {
        max-width: 80%;
        margin: 0 auto;
        font-size: 20px;
        margin-bottom: 40px;
        color: #505050; }
    .background-color .contents-entire .contents-left {
      width: 100%; }
      .background-color .contents-entire .contents-left a {
        display: inline-block;
        position: relative;
        width: 520px; }
        .background-color .contents-entire .contents-left a img {
          width: 520px;
          object-fit: cover;
          vertical-align: bottom;
          margin-bottom: 40px; }
    .background-color .contents-entire .contents-right {
      max-width: 80%;
      margin: 0 auto;
      margin-top: 40px; }
      .background-color .contents-entire .contents-right h3 {
        display: none; }
      .background-color .contents-entire .contents-right h4 {
        font-size: 20px;
        color: #505050;
        margin-bottom: 8px; }
      .background-color .contents-entire .contents-right p {
        font-size: 16px;
        color: #707070;
        margin-bottom: 40px; } }

/*# sourceMappingURL=style.cs.map */
