import React from "react";
import { Row, Col } from "reactstrap";

import ToggleGraph from "./graph";

const Details = ({ match }) => (
  <Row>
    <Col md="6">
      <div>
        <h4>Name</h4>
        <p>Some Feature Toggle {match.params.id}</p>
      </div>
      <div>
        <h4>Description</h4>
        <p>
          Does something very interesting and potentially has quite a long
          description
        </p>
      </div>
      <div>
        <h4>Conditions</h4>
        <p>
          Toggle is active when <b>any</b> | <a href="#">all</a> conditions are
          true.
        </p>

        <ul>
          <li>
            When user is in group <b>Alpha Testers</b>
          </li>
          <li>
            When user is in group <b>Beta Testers</b>
          </li>
          <li>
            When All are True
            <ul>
              <li>
                When user is in group <b>Canary</b>
              </li>
              <li>
                When user is in <b>5%</b> of users
              </li>
            </ul>
          </li>
        </ul>
      </div>
    </Col>
    <Col md="6">
      <ToggleGraph title="Usage Graph" />
      <ToggleGraph title="Activation Graph" />
      <ToggleGraph title="Condition Activation Graph" />
    </Col>
  </Row>
);

export default Details;
