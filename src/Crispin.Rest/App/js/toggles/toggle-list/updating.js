import React from "react";
import { Progress } from "reactstrap";

const Updating = ({ updating }) => updating
  ? (<Progress animated value="100" className="updating rounded-0" />)
  : (<div className=" updating bg-dark" />);

export default Updating;
