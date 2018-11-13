const makeAsync = (prefix, run) => {
  const started = prefix + "_STARTED";
  const finished = prefix + "_FINISHED";

  const func = args => async dispatch => {
    dispatch({ ...args, type: started });

    const result = await run(args);

    dispatch({ ...result, type: finished });
  };

  return { started, finished, action: func };
};

export default makeAsync;
